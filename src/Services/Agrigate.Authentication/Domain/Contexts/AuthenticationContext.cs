using Agrigate.Authentication.Domain.Entities;
using Agrigate.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Authentication.Domain.Contexts;

public class AuthenticationContext : DbContext
{
    public AuthenticationContext(DbContextOptions<AuthenticationContext> options) 
        : base(options)
    {
    }

    // System Tables

    public DbSet<Log> Logs { get; set; }

    // Authentication Tables

    public DbSet<User> Users { get; set; }
}