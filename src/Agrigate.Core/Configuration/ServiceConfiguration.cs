namespace Agrigate.Core.Configuration;

/// <summary>
/// A general configuration used for services
/// </summary>
public class ServiceConfiguration
{
    /// <summary>
    /// The service name used by Akka.Remote
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// The public host name of the service used by Akka.Remote
    /// </summary>
    public string Hostname { get; set; } = string.Empty;

    /// <summary>
    /// The port used by Akka.Remote
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// The port used by Petabridge.CMD
    /// </summary>
    public int CmdPort { get; set; }

    /// <summary>
    /// The host name of the MQTT Broker
    /// </summary>
    public string? MQTTHostname { get; set; }

    /// <summary>
    /// The connection string for the service's database
    /// </summary>
    public string? DatabaseConnection { get; set; }
}