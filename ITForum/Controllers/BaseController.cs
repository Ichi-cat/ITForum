using AutoMapper;
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
        private IMapper _mapper;
        protected IMapper Mapper { get => _mapper ??= HttpContext.RequestServices.GetService<IMapper>(); }
        protected Guid UserId { get; } = Guid.Empty;
    }
}
