using Agrigate.Domain.Contexts;
using Agrigate.IoT.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.IoT.Domain.Contexts;

/// <summary>
/// Database context for the Agrigate IoT service
/// </summary>
public class IoTContext : AgrigateContext
{
    public IoTContext(DbContextOptions<AgrigateContext> options): base(options)
    {
    }

    public DbSet<Device> Devices { get; set; }
    public DbSet<DeviceMethod> DeviceMethods { get; set; }
    public DbSet<Telemetry> Telemetry { get; set; }
}