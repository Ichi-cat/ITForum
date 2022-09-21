using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ITForum.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]/")]
    public abstract class BaseController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator { get => _mediator ??= HttpContext.RequestServices.GetService<IMediator>(); }
        private IMapper _mapper;
        protected IMapper Mapper { get => _mapper ??= HttpContext.RequestServices.GetService<IMapper>(); }
        protected Guid UserId => User.Identity.IsAuthenticated ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)
            : Guid.Empty;
    }
}
