using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ITForum.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public abstract class BaseController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator { get => _mediator ??= HttpContext.RequestServices.GetService<IMediator>(); }
    }
}
