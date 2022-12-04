using AutoMapper;
using ITForum.Domain.ItForumUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ITForum.Api.Controllers
{
    [ApiController]
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
        protected string GenerateAbsoluteUrl(string relativeUrl)
        {
                var request = HttpContext.Request;
                var absoluteUrl = UriHelper.BuildAbsolute(request.Scheme, request.Host, relativeUrl);
                return absoluteUrl;
        }
    }
}
