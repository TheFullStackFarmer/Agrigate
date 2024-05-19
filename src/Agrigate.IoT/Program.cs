// https://github.com/akkadotnet/Akka.Hosting

using Agrigate.IoT;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
