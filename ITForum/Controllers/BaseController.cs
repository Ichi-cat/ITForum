using Microsoft.AspNetCore.Mvc;

namespace ITForum.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public abstract class BaseController : ControllerBase
    {
    }
}
