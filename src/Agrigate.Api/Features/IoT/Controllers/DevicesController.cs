using Agrigate.Api.Core;
using Agrigate.Domain.Messages;
using Akka.Actor;
using Akka.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Agrigate.Api.Controllers.Features.IoT.Controllers;

/// <summary>
/// Controller to handle device-related requests
/// </summary>
public class DevicesController : AgrigateController
{
    public DevicesController(IRequiredActor<ApiSupervisor> supervisor) 
        : base(supervisor)
    {
    }

    /// <summary>
    /// Retrieves a list of devices registered with Agrigate
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetDevices()
    {
        try 
        {
            var result = await ApiSupervisor.Ask(new TestMessage("This is a test message"), TimeSpan.FromSeconds(5));
            return Ok(result);
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return Ok(ex.Message);
        }
    }
}