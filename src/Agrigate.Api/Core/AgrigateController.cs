using Akka.Actor;
using Akka.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Agrigate.Api.Core;

[ApiController]
[Route("[controller]")]
public abstract class AgrigateController : ControllerBase
{
    protected readonly IActorRef ApiSupervisor;

    public AgrigateController(ActorRegistry registry)
    {
        ApiSupervisor = registry.Get<ApiSupervisor>() 
            ?? throw new ArgumentNullException("ApiSupervisor not available!");
    }
}