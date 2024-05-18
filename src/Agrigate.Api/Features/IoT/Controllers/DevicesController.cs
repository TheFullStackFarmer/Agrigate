using Agrigate.Api.Controllers.Features.IoT.Models;
using Agrigate.Api.Core;
using Microsoft.AspNetCore.Mvc;

namespace Agrigate.Api.Controllers.Features.IoT.Controllers;

/// <summary>
/// Controller to handle device-related requests
/// </summary>
public class DevicesController : AgrigateController
{
    public DevicesController() : base()
    {
    }

    /// <summary>
    /// Registers a new device with the Agrigate platform
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult RegisterDevice(DeviceRegistration registration)
    {
        return Ok();
    }

    /// <summary>
    /// Retrieves a list of devices registered with Agrigate
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetDevices()
    {
        return Ok();
    }
}