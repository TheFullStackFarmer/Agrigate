using Agrigate.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Domain.Contexts;

/// <summary>
/// A base context for all Agrigate services
/// </summary>
public abstract class AgrigateContext : DbContext
{
    public AgrigateContext(DbContextOptions<AgrigateContext> options) : base(options)
    {
    }

    // System Tables
    public DbSet<Log> Logs { get; set; }
}