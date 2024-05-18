using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities.IoT;

/// <summary>
/// The definition of a method that can be executed on a device
/// </summary>
[Table(nameof(Device))]
public class DeviceMethod : EntityBase
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The id of the device to which the function belongs
    /// </summary>
    public int DeviceId { get; set; }

    /// <summary>
    /// The name of the function that can be called
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A description of the method's purpose
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// A json formatted array of parameters that the method supports
    /// </summary>
    public string? Parameters { get; set;}
}