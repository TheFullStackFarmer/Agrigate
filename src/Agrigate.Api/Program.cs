using System.Text;
using Agrigate.Api.Core;
using Akka.Actor;
using Akka.Hosting;
using Akka.Remote.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    var jwtSecurityScheme = new OpenApiSecurityScheme 
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put ONLY your JWT token in the textbox below.",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(
        jwtSecurityScheme.Reference.Id,
        jwtSecurityScheme
    );

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var apiConfig = new ApiConfiguration();
builder.Configuration.Bind("Api", apiConfig);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new() 
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = apiConfig.Authentication.Issuer,
            ValidAudience = apiConfig.Authentication.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(apiConfig.Authentication.SecretKey))
        };
    });

builder.Services.AddAkka("API", builder =>
{
    builder
        .WithRemoting(
            hostname: "0.0.0.0",
            publicHostname: apiConfig.Service.Hostname,
            port: apiConfig.Service.Port
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
