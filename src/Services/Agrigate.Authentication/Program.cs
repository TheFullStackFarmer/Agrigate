using Agrigate.Authentication.Actors;
using Agrigate.Authentication.Domain.Contexts;
using Agrigate.Core.Configuration;
using Akka.Hosting;
using Akka.Remote.Hosting;
using Microsoft.EntityFrameworkCore;
using Petabridge.Cmd.Host;
using Petabridge.Cmd.Remote;

var builder = Host.CreateApplicationBuilder(args);

//////////////////////////////////////////
//          Configure Settings          //
//////////////////////////////////////////

var serviceConfig = new ServiceConfiguration();
builder.Configuration.Bind("Service", serviceConfig);

//////////////////////////////////////////
//            Database Setup            //
//////////////////////////////////////////

builder.Services.AddDbContextFactory<AuthenticationContext>(options =>
    options.UseNpgsql(serviceConfig.DatabaseConnection));

//////////////////////////////////////////
//               Akka.Net               //
//////////////////////////////////////////

builder.Services.AddAkka("AuthenticationService", builder =>
{
    builder
        .WithRemoting(
            hostname: "0.0.0.0",
            publicHostname: serviceConfig.Hostname,
            port: int.Parse(serviceConfig.Port)
        )
        .WithActors((system, registry, resolver) =>
        {
            var supervisorProps = resolver.Props<AuthenticationSupervisor>();
            var supervisor = system.ActorOf(supervisorProps, "Supervisor");
            registry.Register<AuthenticationSupervisor>(supervisor);
        })
        .AddPetabridgeCmd(
            new PetabridgeCmdOptions
            {
                Host = "0.0.0.0",
                Port = int.Parse(serviceConfig.CmdPort ?? "0")
            },
            cmd =>
            {
                cmd.RegisterCommandPalette(new RemoteCommands());
            }
        );
});

var host = builder.Build();
host.Run();
