using Agrigate.Api.Core;
using Agrigate.Core;
using Agrigate.Domain.Messages.IoT;
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
                new GetDevices(), 
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

    /// <summary>
    /// Retrieves information about a particular device
    /// </summary>
    /// <param name="deviceId">The DeviceId to query for</param>
    /// <returns></returns>
    [HttpGet("{deviceId}")]
    public async Task<IActionResult> GetDevice(string deviceId)
    {
        try
        {
            var result = await ApiSupervisor.Ask(
                new GetDevices(deviceId),
                Constants.MaxActorWaitTime
            );

            if (result is Exception exception)
                throw exception;

            return Success(result);
        }
        catch (Exception ex)
        {
            return Failure($"Unable to retrieve device: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves telemetry for a specific DeviceId
    /// </summary>
    /// <param name="deviceId">The DeviceId to query for</param>
    /// <param name="startTime">The earliest UTC date time for which to retrieve telemetry</param>
    /// <param name="endTime">The latest UTC date time for which to retrieve telemetry</param>
    /// <returns></returns>
    [HttpGet("{deviceId}/Telemetry")]
    public async Task<IActionResult> GetDeviceTelemetry(
        string deviceId,
        [FromQuery] DateTimeOffset? startTime,
        [FromQuery] DateTimeOffset? endTime
    )
    {
        try
        {
            var result = await ApiSupervisor.Ask(
                new GetTelemtry(deviceId, startTime, endTime),
                Constants.MaxActorWaitTime
            );

            if (result is Exception exception)
                throw exception;

            return Success(result);
        }
        catch (Exception ex)
        {
            return Failure($"Unable to retrieve telemetry: {ex.Message}");
        }
    }
}