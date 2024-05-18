using Agrigate.Domain.Entities.IoT;
using Agrigate.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Domain.Contexts;

public class AgrigateContext : DbContext
{
    public AgrigateContext(DbContextOptions<AgrigateContext> options) : base(options)
    {
    }

    // System Tables
    public DbSet<Log> Logs { get; set; }

    // IoT Tables
    public DbSet<Device> Devices { get; set; }
    public DbSet<DeviceMethod> DeviceMethods { get; set; }
    public DbSet<Telemetry> Telemetry { get; set; }
}