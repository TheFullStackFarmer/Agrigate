using Microsoft.AspNetCore.Mvc;

namespace Agrigate.Api.Core;

[ApiController]
[Route("[controller]")]
public abstract class AgrigateController : ControllerBase
{
    public AgrigateController()
    {
    }
}