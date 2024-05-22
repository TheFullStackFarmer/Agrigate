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
}