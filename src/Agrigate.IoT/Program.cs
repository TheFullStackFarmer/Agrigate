using Agrigate.Core.Configuration;
using Agrigate.IoT.Actors;
using Akka.Actor;
using Akka.Hosting;
using Akka.Remote.Hosting;
using Petabridge.Cmd.Host;
using Petabridge.Cmd.Remote;

var builder = Host.CreateApplicationBuilder(args);

var serviceConfig = new ServiceConfiguration();
builder.Configuration.Bind("Service", serviceConfig);

builder.Services.Configure<ServiceConfiguration>(
    builder.Configuration.GetSection("Service"));

builder.Services.AddAkka("IoTService", builder =>
{
    builder
        .WithRemoting(
            hostname: "0.0.0.0",
            publicHostname: serviceConfig.Hostname,
            port: int.Parse(serviceConfig.Port)
        )
        .WithActors((system, registry) =>
        {
            var supervisor = system.ActorOf(Props.Create(
                () => new IoTSupervisor()), 
                "Supervisor"
            );

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
