using Agrigate.Domain.Entities.System;
using Agrigate.IoT.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.IoT.Domain.Contexts;

/// <summary>
/// Database context for the Agrigate IoT service
/// </summary>
public class IoTContext : DbContext
{
    public IoTContext(DbContextOptions<IoTContext> options): base(options)
    {
    }

    // System Tables

    public DbSet<Log> Logs { get; set; }

    // IoT Tables

    public DbSet<Device> Devices { get; set; }
    public DbSet<DeviceMethod> DeviceMethods { get; set; }
    public DbSet<Telemetry> Telemetry { get; set; }
}