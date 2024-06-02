using Agrigate.Core.Actors;
using Agrigate.Core.Configuration;
using Agrigate.IoT.Domain.DTOs;
using Akka.Actor;
using Akka.Event;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;

/// <summary>
/// A base actor that contains logic for connecting to an MQTT broker
/// </summary>
public abstract class MQTTActor : AgrigateActor
{
    protected readonly ILoggingAdapter Log;
    protected readonly ServiceConfiguration Configuration;
    protected readonly IUntypedActorContext ActorContext;

    protected MqttFactory? MqttFactory;
    protected IMqttClient? MqttClient;
    private MqttClientOptions? _options;

    public MQTTActor(IOptions<ServiceConfiguration> options)
    {
        Log = Logging.GetLogger(Context) ?? throw new ApplicationException("Unable to retrieve logger");
        Configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
        ActorContext = Context;
    }

    protected override void PostStop()
    {
        DisconnectFromBroker();
        DisposeBrokerConnection();
    }

    /// <summary>
    /// Connects to the MQTT broker with the given client id and message handler
    /// </summary>
    /// <param name="clientId">The clientId to use when connecting to the broker</param>
    /// <param name="messageHandler">The event handler called when receiving a message</param>
    protected void ConnectToBroker(
        string clientId, 
        Func<MqttApplicationMessageReceivedEventArgs, Task>? messageHandler = null
    )
    {
        try 
        {
            MqttFactory = new MqttFactory();
            _options = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithTcpServer(Configuration.MQTTHostname)
                .Build();
            MqttClient = MqttFactory.CreateMqttClient();

            MqttClient.DisconnectedAsync += Reconnect;

            if (messageHandler != null)
                MqttClient.ApplicationMessageReceivedAsync += messageHandler;

            MqttClient.ConnectAsync(_options, CancellationToken.None).Wait();
        }
        catch (Exception ex)
        {
            Log.Error($"Unable to connect to broker: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Subscribes to a particular topic
    /// </summary>
    /// <exception cref="ApplicationException"></exception>
    protected void SubscribeToTopic(string topic)
    {
        if (MqttFactory == null || MqttClient == null)
        {
            var message = "MQTT Factory or Client failed to initialize";
            Log.Error(message);
            throw new ApplicationException(message);
        }

        try
        {
            var subscriptionOptions = MqttFactory
                .CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => { f.WithTopic(topic); })
                .Build();

            MqttClient.SubscribeAsync(subscriptionOptions, CancellationToken.None).Wait();
        }
        catch (Exception ex)
        {
            Log.Error($"Unable to subscribe to client events: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Attempts to publish a message to the MQTT Broker
    /// </summary>
    /// <param name="topic">The topic that should be published to</param>
    /// <param name="payload">The payload</param>
    protected void PublishMessage(string topic, DeviceMessage payload)
    {
        if (MqttClient == null)
        {
            Log.Error($"Unable to publish message. MQTTClient not initialized");
            return;
        }

        try
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(JsonConvert.SerializeObject(payload))
                .Build();

            MqttClient.PublishAsync(message, CancellationToken.None).Wait();
        }
        catch (Exception ex)
        {
            Log.Error($"Error publishing message: {ex.Message}");
        }
    }

    /// <summary>
    /// Attempts to reconnect to the message broker
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    private async Task Reconnect(MqttClientDisconnectedEventArgs e)
    {
        if (MqttClient == null || _options == null)
            return;

        if (e.ClientWasConnected)
            await MqttClient.ConnectAsync(_options);
    }

    /// <summary>
    /// Cleanly disconnect from the MQTT broker
    /// </summary>
    /// <exception cref="ApplicationException"></exception>
    private void DisconnectFromBroker()
    {
        try 
        {
            if (MqttClient == null)
                throw new ApplicationException("MQTT Client was not initialized");
                
            var disconnectOptions = new MqttClientDisconnectOptionsBuilder()
                .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection)
                .Build();

            MqttClient.DisconnectAsync(disconnectOptions, CancellationToken.None).Wait();
        }
        catch (Exception ex)
        {
            Log.Error($"Unable to disconnect from broker: {ex.Message}");
        }
    }

    /// <summary>
    /// Disposes of the MQTT client, if it exists
    /// </summary>
    private void DisposeBrokerConnection()
    {
        MqttClient?.Dispose();
    }
}