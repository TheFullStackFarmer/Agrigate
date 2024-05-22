using Agrigate.Core.Configuration;
using Akka.Event;
using Microsoft.Extensions.Options;
using MQTTnet.Client;

namespace Agrigate.IoT.Actors.Devices;

/// <summary>
/// An actor representing a single device connected to the Agrigate platform
/// </summary>
public class Device : MQTTActor
{
    private readonly string _deviceId;

    public Device(IOptions<ServiceConfiguration> options, string deviceId) : base(options)
    {
        _deviceId = deviceId ?? throw new ArgumentNullException(nameof(deviceId));
    }

    protected override void PreStart()
    {
        Log.Info($"Device {_deviceId} initializing...");

        ConnectToBroker($"agrigate-{_deviceId}", HandleDeviceEvent);
        SubscribeToTopic($"devices/{_deviceId}");
        // TODO: Query & Create device in database

        Log.Info($"Device {_deviceId} ready.");
    }

    protected override void PostStop()
    {
        base.PostStop();
        Log.Info($"Device {_deviceId} terminated.");
    }

    private Task HandleDeviceEvent(MqttApplicationMessageReceivedEventArgs e)
    {
        try
        {
            // TODO: Handle events - only telemetry at the moment
            var message = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            Log.Info(message);

            // var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(connectionEvent?.Timestamp ?? 0);
            // DateTime.TryParse(connectionEvent.ConnectedAt, out var time))
        }
        catch (Exception ex)
        {
            Log.Error($"Unable to process device event: {ex.Message}");
        }

        return Task.CompletedTask;
    }
}