using Newtonsoft.Json;

namespace Agrigate.IoT.Domain.DTOs;

/// <summary>
/// An event emitted by the MQTT Broker when a device connects or disconnects
/// </summary>
public class BrokerConnectionEvent
{
    public string IpAddress { get; set; } = string.Empty;

    [JsonProperty("expiry_interval")]
    public int? ExpiryInterval { get; set; }

    [JsonProperty("clean_start")]
    public bool? CleanStart { get; set; }

    [JsonProperty("sockport")]
    public int Port { get; set; }

    [JsonProperty("connected_at")]
    public long? ConnectedAt { get; set; }

    [JsonProperty("disconnected_at")]
    public long? DisconnecteAt { get; set; }

    [JsonProperty("proto_ver")]
    public int ProtocolVersion { get; set; }

    [JsonProperty("proto_name")]
    public string ProtocolName { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    
    [JsonProperty("ts")]
    public long Timestamp { get; set; }

    public string? Reason { get; set; }
    public string Protocol { get; set; } = string.Empty;
    public int? KeepAlive { get; set; }
}