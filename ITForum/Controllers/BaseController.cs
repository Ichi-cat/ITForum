using AutoMapper;
using ITForum.Api.Models.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ITForum.Controllers
{
    [ApiController]
    //[Authorize(Roles = UserRoles.User)]
    [Route("[controller]/")]
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator { get => _mediator ??= HttpContext.RequestServices.GetService<IMediator>(); }
        private IMapper _mapper;
        protected IMapper Mapper { get => _mapper ??= HttpContext.RequestServices.GetService<IMapper>(); }
        protected Guid UserId => User.Identity.IsAuthenticated ? Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Jti).Value)
            : Guid.Empty;
    }
}
