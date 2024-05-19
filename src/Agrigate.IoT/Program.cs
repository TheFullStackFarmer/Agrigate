// https://github.com/akkadotnet/Akka.Hosting

using Agrigate.IoT.Actors;
using Akka.Actor;
using Akka.Hosting;
using Akka.Remote.Hosting;
using Petabridge.Cmd.Host;
using Petabridge.Cmd.Remote;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddAkka("IoTService", builder =>
{
    builder
        .WithRemoting(
            hostname: "0.0.0.0",
            publicHostname: "iot",
            port: 5000
        )
        .WithActors((system, registry) =>
        {
            var supervisor = system.ActorOf(Props.Create(() => new IoTSupervisor()), "Supervisor");
            registry.Register<IoTSupervisor>(supervisor);
        })
        .AddPetabridgeCmd(
            new PetabridgeCmdOptions 
            {
                Host = "127.0.0.1",
                Port = 5001
            },
            cmd => 
            {
                cmd.RegisterCommandPalette(new RemoteCommands());
            }
        );
});

var host = builder.Build();
host.Run();
