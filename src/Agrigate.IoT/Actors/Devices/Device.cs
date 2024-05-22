using Agrigate.Core.Configuration;
using Akka.Actor;
using Akka.Event;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;

namespace Agrigate.IoT.Actors.Devices;

/// <summary>
/// An actor representing a single device connected to the Agrigate platform
/// </summary>
public class Device : ReceiveActor
{
    private readonly ILoggingAdapter _log;
    private readonly ServiceConfiguration _configuration;

    private readonly string _deviceId;
    private readonly MqttFactory _mqttFactory;

    private IMqttClient? _mqttClient;
    private MqttClientOptions? _options;

    public Device(IOptions<ServiceConfiguration> options, string deviceId, MqttFactory mqttFactory)
    {
        _log = Logging.GetLogger(Context) ?? throw new ApplicationException("Unable to retrieve logger");
        _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
        _deviceId = deviceId ?? throw new ArgumentNullException(nameof(deviceId));
        _mqttFactory = mqttFactory ?? throw new ArgumentNullException(nameof(mqttFactory));
    }

    protected override void PreStart()
    {
        _log.Info($"Device {_deviceId} initializing...");

        ConnectToBroker();
        SubscribeToDeviceEvents();
        // TODO: Query & Create device in database

        _log.Info($"Device {_deviceId} ready.");
    }

    protected override void PostStop()
    {
        DisconnectFromBroker();
        DisposeBrokerConnection();
        _log.Info($"Device {_deviceId} terminated.");
    }

    private void ConnectToBroker()
    {
        try 
        {
            _options = new MqttClientOptionsBuilder()
                .WithClientId($"agrigate-{_deviceId}")
                .WithTcpServer(_configuration.MQTTHostname)
                .Build();
            _mqttClient = _mqttFactory.CreateMqttClient();

            _mqttClient.DisconnectedAsync += Reconnect;
            _mqttClient.ApplicationMessageReceivedAsync += HandleDeviceEvent;

            _mqttClient.ConnectAsync(_options, CancellationToken.None).Wait();
        }
        catch (Exception ex)
        {
            _log.Error($"Unable to connect to broker: {ex.Message}");
            throw;
        }
    }

    private void DisconnectFromBroker()
    {
        try 
        {
            if (_mqttClient == null)
                throw new ApplicationException("MQTT Client was not initialized");

            var disconnectOptions = new MqttClientDisconnectOptionsBuilder()
                .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection)
                .Build();

            _mqttClient.DisconnectAsync(disconnectOptions, CancellationToken.None).Wait();
        }
        catch (Exception ex)
        {
            _log.Error($"Unable to disconnect from broker: {ex.Message}");
        }
    }

    private void DisposeBrokerConnection()
    {
        _mqttClient?.Dispose();
    }

    private void SubscribeToDeviceEvents()
    {
        if (_mqttClient == null)
        {
            var message = "MQTT Client failed to initialize";
            _log.Error(message);
            throw new ApplicationException(message);
        }

        try 
        {
            var subscriptionOptions = _mqttFactory
                .CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => { f.WithTopic($"devices/{_deviceId}"); })
                .Build();

            _mqttClient.SubscribeAsync(subscriptionOptions, CancellationToken.None).Wait();
        }
        catch (Exception ex)
        {
            _log.Error($"Unable to subscribe to device events: {ex.Message}");
            throw;
        }
    }

    private async Task Reconnect(MqttClientDisconnectedEventArgs e)
    {
        if (_mqttClient == null || _options == null)
            return;

        if (e.ClientWasConnected)
            await _mqttClient.ConnectAsync(_options);
    }

    private Task HandleDeviceEvent(MqttApplicationMessageReceivedEventArgs e)
    {
        try
        {
            // TODO: Handle events - only telemetry at the moment
            var message = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            _log.Info(message);

            // var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(connectionEvent?.Timestamp ?? 0);
            // DateTime.TryParse(connectionEvent.ConnectedAt, out var time))
        }
        catch (Exception ex)
        {
            _log.Error($"Unable to process device event: {ex.Message}");
        }

        return Task.CompletedTask;
    }
}