using System.Collections.Concurrent;
using Agrigate.Core.Configuration;
using Agrigate.IoT.Domain.DTOs;
using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using Akka.Util.Internal;
using Microsoft.Extensions.Options;
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

    private readonly IActorContext _context;
    private ConcurrentDictionary<string, IActorRef> _deviceActors;

    public DeviceManager(IOptions<ServiceConfiguration> options)
    {
        _log = Logging.GetLogger(Context) ?? throw new ApplicationException("Unable to retrieve logger");
        _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));

        _deviceActors = new ConcurrentDictionary<string, IActorRef>();
        _context = Context;
    }

    protected override void PreStart()
    {
        ConnectToBroker();
        SubscribeToConnectionEvents();
    }

    protected override void PostStop()
    {
        DisconnectFromBroker();
        DisposeBrokerConnection();
    }

    private void ConnectToBroker()
    {
        try 
        {
            _mqttFactory = new MqttFactory();
            _options = new MqttClientOptionsBuilder()
                .WithClientId("agrigate-device-manager")
                .WithTcpServer(_configuration.MQTTHostname)
                .Build();
            _mqttClient = _mqttFactory.CreateMqttClient();

            _mqttClient.DisconnectedAsync += Reconnect;
            _mqttClient.ApplicationMessageReceivedAsync += HandleConnectionEvent;

            _mqttClient.ConnectAsync(_options, CancellationToken.None).Wait();
        }
        catch (Exception ex)
        {
            _log.Error($"Unable to connect to broker: {ex.Message}");
            throw;
        }
    }

    private void SubscribeToConnectionEvents()
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

    // Event Handlers

    private async Task Reconnect(MqttClientDisconnectedEventArgs e)
    {
        if (_mqttClient == null || _options == null)
            return;

        if (e.ClientWasConnected)
            await _mqttClient.ConnectAsync(_options);
    }

    private Task HandleConnectionEvent(MqttApplicationMessageReceivedEventArgs e)
    {
        try 
        {
            var message = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            var connectionEvent = JsonConvert.DeserializeObject<BrokerConnectionEvent>(message) 
                ?? throw new ApplicationException("Unable to parse connection event");

            var clientId = connectionEvent.ClientId;

            // Don't register a new actor for Agrigate actors
            if (clientId.Contains("agrigate"))
                return Task.CompletedTask;
            
            if (connectionEvent.DisconnecteAt != null) 
            {
                if (!_deviceActors.TryGetValue(clientId, out var actorRef))
                {
                    _log.Warning($"{clientId} is not registered");
                    return Task.CompletedTask;
                }

                _deviceActors.Remove(clientId, out actorRef);
                actorRef.Tell(PoisonPill.Instance);
            }

            else if (connectionEvent.ConnectedAt != null)
            {
                if (_mqttFactory == null)
                    throw new ApplicationException("MQTT Factory is unavailable");

                if (_deviceActors.TryGetValue(clientId, out var actorRef))
                {
                    _log.Warning($"{clientId} already registered with {actorRef}");
                    return Task.CompletedTask;
                }
                
                var deviceProps = DependencyResolver.For(_context.System).Props<Device>(clientId, _mqttFactory);
                var deviceActor = _context.ActorOf(deviceProps, $"Device-{clientId}");

                _deviceActors.AddOrSet(clientId, deviceActor);
            }
        }
        catch (Exception ex)
        {
            _log.Error($"Unable to process connection event: {ex.Message}");
        }

        return Task.CompletedTask;
    }
}