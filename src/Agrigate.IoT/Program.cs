using Agrigate.Core.Configuration;
using Agrigate.IoT.Actors;
using Agrigate.IoT.Domain.Contexts;
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

builder.Services.Configure<ServiceConfiguration>(
    builder.Configuration.GetSection("Service"));

//////////////////////////////////////////
//            Database Setup            //
//////////////////////////////////////////

builder.Services.AddDbContextFactory<IoTContext>(options =>
    options.UseNpgsql(serviceConfig.DatabaseConnection));

//////////////////////////////////////////
//               Akka.Net               //
//////////////////////////////////////////

builder.Services.AddAkka("IoTService", builder =>
{
    builder
        .WithRemoting(
            hostname: "0.0.0.0",
            publicHostname: serviceConfig.Hostname,
            port: int.Parse(serviceConfig.Port)
        )
        .WithActors((system, registry, resolver) =>
        {
            var supervisorProps = resolver.Props<IoTSupervisor>();
            var supervisor = system.ActorOf(supervisorProps, "Supervisor");
            registry.Register<IoTSupervisor>(supervisor);
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
