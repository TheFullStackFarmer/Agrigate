using Agrigate.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.IoT.Domain.Entities;

/// <summary>
/// Represents a single reading from any device interface
/// </summary>
[Table(nameof(Telemetry))]
public class Telemetry : EntityBase
{
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// The Id of the device to which the telemetry reading belongs
    /// </summary>
    public int DeviceId { get; set; }

    /// <summary>
    /// The time at which the telemetry was recorded
    /// </summary>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// The identifier for the value being recorded
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// A numeric value received from the interface
    /// </summary>
    public double? Value { get; set; }

    /// <summary>
    /// A boolean value received from the interface
    /// </summary>
    public bool? BoolValue { get; set; }

    /// <summary>
    /// A string value received from the interface
    /// </summary>
    public string? StringValue { get; set; }
}