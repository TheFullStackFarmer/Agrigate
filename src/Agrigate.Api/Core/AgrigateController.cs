using Akka.Actor;
using Akka.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Agrigate.Api.Core;

/// <summary>
/// A base controller for the Agrigate API
/// </summary>
[ApiController]
[Route("[controller]")]
public abstract class AgrigateController : ControllerBase
{
    protected readonly IActorRef ApiSupervisor;

    public AgrigateController(IRequiredActor<ApiSupervisor> supervisor)
    {
        ApiSupervisor = supervisor.ActorRef
            ?? throw new ArgumentNullException(nameof(supervisor));
    }

    /// <summary>
    /// Returns a successful Agrigate API response
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    protected IActionResult Success(object data)
        => ApiResponse.Success(data);
    
    /// <summary>
    /// Returns a created Agrigate API response
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    protected IActionResult Created(object data)
        => ApiResponse.Created(data);

    /// <summary>
    /// Returns a failed Agrigate API response
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    protected IActionResult Failure(string error)
        => ApiResponse.Failure(error);

    /// <summary>
    /// Returns an unauthorized Agrigate API response
    /// </summary>
    /// <returns></returns>
    protected IActionResult UnauthorizedAgrigateRequest()
        => ApiResponse.Unauthorized();

    /// <summary>
    /// Returns a bad request Agrigate response
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    protected IActionResult BadAgrigateRequest(string error)
        => ApiResponse.BadRequest(error);
}