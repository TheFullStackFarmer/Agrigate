using Agrigate.Core.Configuration;
using Agrigate.IoT.Domain.DTOs;
using Akka.Actor;
using Akka.Event;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;

namespace Agrigate.IoT.Actors.Devices;

/// <summary>
/// The actor responsible for managing devices connected to Agrigate
/// </summary>
public class DeviceManager : ReceiveActor
{
    private readonly ILoggingAdapter _log;
    private readonly ServiceConfiguration _configuration;

    private MqttFactory? _mqttFactory;
    private IMqttClient? _mqttClient;
    private MqttClientOptions? _options;

    public DeviceManager(ServiceConfiguration configuration)
    {
        _log = Logging.GetLogger(Context) ?? throw new ApplicationException("Unable to retrieve logger");
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    protected override void PreStart()
    {
        ConnectToBroker();
        SubscribeToClientEvents();
    }

    protected override void PostStop()
    {
        DisposeBrokerConnection();
    }

    private void ConnectToBroker()
    {
        try 
        {
            _mqttFactory = new MqttFactory();
            _options = new MqttClientOptionsBuilder()
                .WithClientId("device-manager")
                .WithTcpServer(_configuration.MQTTHostname)
                .Build();
            _mqttClient = _mqttFactory.CreateMqttClient();

            _mqttClient.DisconnectedAsync += Reconnect;
            _mqttClient.ApplicationMessageReceivedAsync += HandleEvent;

            _mqttClient.ConnectAsync(_options, CancellationToken.None).Wait();
        }
        catch (Exception ex)
        {
            _log.Error($"Unable to connect to broker: {ex.Message}");
            throw;
        }
    }

    private void SubscribeToClientEvents()
    {
        if (_mqttFactory == null || _mqttClient == null)
        {
            var message = "MQTT Factory or Client failed to initialize";
            _log.Error(message);
            throw new ApplicationException(message);
        }

        try
        {
            var subscriptionOptions = _mqttFactory
                .CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => { f.WithTopic(@"$SYS/brokers/+/clients/#"); })
                .Build();

            _mqttClient.SubscribeAsync(subscriptionOptions, CancellationToken.None).Wait();
        }
        catch (Exception ex)
        {
            _log.Error($"Unable to subscribe to client events: {ex.Message}");
            throw;
        }
    }

    private void DisposeBrokerConnection()
    {
        _mqttClient?.Dispose();
    }

    // Event Handlers

    private async Task Reconnect(MqttClientDisconnectedEventArgs e)
    {
        if (_mqttClient == null || _options == null)
            return;

        if (e.ClientWasConnected)
            await _mqttClient.ConnectAsync(_options);
    }

    private Task HandleEvent(MqttApplicationMessageReceivedEventArgs e)
    {
        try 
        {
            _log.Info("Received application message");
            var message = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            var connectionEvent = JsonConvert.DeserializeObject<BrokerConnectionEvent>(message) 
                ?? throw new ApplicationException("Unable to parse connection event");

            if (connectionEvent.ConnectedAt != null)
            {
                // TODO: Create actor if it doesn't exist
            }
            else if (connectionEvent.DisconnecteAt != null) 
            {
                // TODO: Kill actor if it exists
            }
            else
            {
                _log.Warning($"Event not handled: {connectionEvent}");
            }

            // var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(connectionEvent?.Timestamp ?? 0);
            // DateTime.TryParse(connectionEvent.ConnectedAt, out var time))
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _log.Error($"Unable to process application message: {ex.Message}");
            return Task.CompletedTask;
        }
    }
}