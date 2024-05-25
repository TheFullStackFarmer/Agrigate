using Agrigate.Api.Core;
using Agrigate.Core;
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
            var result = await ApiSupervisor.Ask(
                new Domain.Messages.IoT.GetDevices(), 
                Constants.MaxActorWaitTime
            );

            if (result is Exception exception)
                throw exception;

            return Success(result);
        }
        catch (Exception ex) 
        {
            return Failure($"Unable to retrieve devices: {ex.Message}");
        }
    }
}