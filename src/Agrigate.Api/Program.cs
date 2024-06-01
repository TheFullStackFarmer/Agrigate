using Agrigate.Api.Core;
using Akka.Actor;
using Akka.Hosting;
using Akka.Remote.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var apiConfig = new ApiConfiguration();
builder.Configuration.Bind("Api", apiConfig);

builder.Services.AddAkka("API", builder =>
{
    builder
        .WithRemoting(
            hostname: "0.0.0.0",
            publicHostname: apiConfig.Service.Hostname,
            port: int.Parse(apiConfig.Service.Port)
        )
        .WithActors((system, registry) =>
        {
            var supervisor = system.ActorOf(Props.Create(() => new ApiSupervisor(apiConfig)), "Supervisor");
            registry.Register<ApiSupervisor>(supervisor);
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
