using Agrigate.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.IoT.Domain.Entities;

/// <summary>
/// The representation of a physical device that can communicate with Agrigate
/// </summary>
[Table(nameof(Device))]
public class Device : EntityBase
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// A key that should be supplied by the device when making requests to 
    /// identify itself
    /// </summary>
    public Guid DeviceKey { get; set; }

    /// <summary>
    /// The identifier of the device
    /// </summary>
    public string DeviceId { get; set;} = string.Empty;

    /// <summary>
    /// The model of the device
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// The serial number of the device
    /// </summary>
    public string? SerialNumber { get; set; }

    /// <summary>
    /// The last time the device connected to Agrigate
    /// </summary>
    public DateTimeOffset LastConnection { get; set; }

    /// <summary>
    /// A collection of methods that can be executed remotely on the device
    /// </summary>
    public ICollection<DeviceMethod>? Methods { get; set; }
}