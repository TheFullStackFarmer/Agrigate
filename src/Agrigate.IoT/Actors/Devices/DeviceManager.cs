using System.Collections.Concurrent;
using Agrigate.Core.Configuration;
using Agrigate.Domain.Messages.IoT;
using Agrigate.IoT.Domain.DTOs;
using Agrigate.IoT.Domain.Messages;
using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using Akka.Util.Internal;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using Newtonsoft.Json;

namespace Agrigate.IoT.Actors.Devices;

/// <summary>
/// The actor responsible for managing devices connected to Agrigate
/// </summary>
public class DeviceManager : MQTTActor
{
    private readonly IActorContext _context;
    private ConcurrentDictionary<string, IActorRef> _deviceActors;

    public DeviceManager(IOptions<ServiceConfiguration> options) : base(options)
    {
        _deviceActors = new ConcurrentDictionary<string, IActorRef>();
        _context = Context;

        Receive<GetDevices>(HandleGetDevices);
    }

    protected override void PreStart()
    {
        ConnectToBroker("agrigate-device-manager", HandleConnectionEvent);
        SubscribeToTopic(@"$SYS/brokers/+/clients/#");
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
                    Log.Warning($"{clientId} is not registered");
                    return Task.CompletedTask;
                }

                _deviceActors.Remove(clientId, out actorRef);
                actorRef.Tell(PoisonPill.Instance);
            }

            else if (connectionEvent.ConnectedAt != null)
            {
                if (MqttFactory == null)
                    throw new ApplicationException("MQTT Factory is unavailable");

                if (_deviceActors.TryGetValue(clientId, out var actorRef))
                {
                    Log.Warning($"{clientId} already registered with {actorRef}");
                    return Task.CompletedTask;
                }
                
                var deviceProps = DependencyResolver.For(_context.System).Props<Device>(clientId);
                var deviceActor = _context.ActorOf(deviceProps, $"Device-{clientId}");

                _deviceActors.AddOrSet(clientId, deviceActor);
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Unable to process connection event: {ex.Message}");
        }

        return Task.CompletedTask;
    }

    public void HandleGetDevices(GetDevices message)
    {
        var connectedDevices = _deviceActors.Keys.ToList();
        AskFor(new DeviceRetrieval(connectedDevices));
    }
}