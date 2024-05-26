using Agrigate.Core;
using Agrigate.Core.Configuration;
using Agrigate.IoT.Domain.DTOs;
using Agrigate.IoT.Domain.Messages;
using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using Newtonsoft.Json;

namespace Agrigate.IoT.Actors.Devices;

/// <summary>
/// An actor representing a single device connected to the Agrigate platform
/// </summary>
public class Device : MQTTActor, IWithTimers
{
    public ITimerScheduler Timers { get; set; } = null!;
    private readonly Cancelable _timerCancelable;

    private readonly string _deviceId;
    private readonly string _agrigateDeviceId;
    private readonly string _publishTopic;
    private readonly string _subscribeTopic;

    private Guid? _deviceKey;

    public Device(IOptions<ServiceConfiguration> options, string deviceId) 
        : base(options)
    {
        _timerCancelable = new Cancelable(Context.System.Scheduler);

        _deviceId = deviceId ?? throw new ArgumentNullException(nameof(deviceId));
        _agrigateDeviceId = $"agrigate-{_deviceId}";
        _publishTopic = $"devices/{_deviceId}";
        _subscribeTopic = $"devices/{_deviceId}/messages";

        Receive<PublishMessage>(HandlePublishMessage);
    }

    protected override void PreStart()
    {
        Log.Info($"Device {_deviceId} initializing...");

        InitializeDevice();
        ConnectToBroker(_agrigateDeviceId, HandleDeviceEvent);
        SubscribeToTopic(_subscribeTopic);
        PublishDeviceKey();

        Log.Info($"Device {_deviceId} ready.");
    }

    protected override void PostStop()
    {
        base.PostStop();
        _timerCancelable.Cancel();
        Log.Info($"Device {_deviceId} terminated.");
    }

    /// <summary>
    /// Initializes the device in the IoT database
    /// </summary>
    private void InitializeDevice()
    {
        var queryProps = DependencyResolver.For(Context.System).Props<DeviceQueryActor>();
        var queryHandler = Context.ActorOf(queryProps);

        Domain.Entities.Device? device = queryHandler
            .Ask(new DeviceConnect(_deviceId), Constants.MaxActorWaitTime)
            .GetAwaiter()
            .GetResult() as Domain.Entities.Device;

        if (device == null)
        {
            Log.Error($"Unable to intialize device {_deviceId}");
            Self.Tell(PoisonPill.Instance);
        }

        _deviceKey = device?.DeviceKey;
    }

    /// <summary>
    /// Publishes the DeviceKey to the associated device, so it can be used in 
    /// future requests and messages
    /// </summary>
    private void PublishDeviceKey()
    {
        var payload = new DeviceMessage
        {
            Timestamp = DateTimeOffset.UtcNow,
            MessageId = Guid.NewGuid(),
            MessageType = DeviceMessageType.DeviceKey,
            Payload = _deviceKey.ToString(),
            ExpectResponse = false
        };

        // Give the connected device time to subscribe to the appropriate topic
        // ScheduleTellOnce() and ScheduleTellRepeatedly() can cause memory leak 
        // if not properly canceled - https://getakka.net/articles/debugging/rules/AK1004.html
        #pragma warning disable AK1004 
        Context.System.Scheduler.ScheduleTellOnce(
            delay: TimeSpan.FromSeconds(1),
            receiver: Self,
            message: new PublishMessage(_publishTopic, payload),
            sender: Self,
            cancelable: _timerCancelable
        );
        #pragma warning restore AK1004
    }

    /// <summary>
    /// Handler for the PublishMessage record. Publishes the provided payload to 
    /// the specified topic
    /// </summary>
    /// <param name="message"></param>
    private void HandlePublishMessage(PublishMessage message)
    {
        PublishMessage(message.Topic, message.Payload);
    }

    /// <summary>
    /// Handles incomming messages from the {deviceId} device
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    private Task HandleDeviceEvent(MqttApplicationMessageReceivedEventArgs e)
    {
        try
        {
            var message = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            var telemetry = JsonConvert.DeserializeObject<DeviceTelemetry>(message);
            
            if (telemetry == null) 
            {
                Log.Warning($"Unable to deserialize message: {message}");
                return Task.CompletedTask;
            }

            if (telemetry.Payload == null)
            {
                Log.Warning($"No payload detected: {message}");
                return Task.CompletedTask;
            }

            var couldParse = Guid.TryParse(telemetry.DeviceKey, out var deviceKey);
            if (!couldParse || deviceKey != _deviceKey)
            {
                Log.Warning($"Unable to verify message was from {_deviceId}: {message}");
                return Task.CompletedTask;
            }

            var queryProps = DependencyResolver.For(ActorContext.System).Props<DeviceQueryActor>();
            var queryHandler = ActorContext.ActorOf(queryProps);
            
            queryHandler.Tell(new TelemetryReceived(_deviceId, telemetry.Payload));
        }
        catch (Exception ex)
        {
            Log.Error($"Unable to process device event: {ex.Message}");
        }

        return Task.CompletedTask;
    }
}