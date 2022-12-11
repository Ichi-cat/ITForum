using ITForum.Api.enums;
using ITForum.Api.Models;
using ITForum.Application.Common.Exceptions;
using ITForum.Application.Interfaces;
using ITForum.Application.Users.Commands.DeleteUser;
using ITForum.Application.Users.Commands.SubscribeOnUser;
using ITForum.Application.Users.Commands.UnsubscribeFromUser;
using ITForum.Application.Users.Commands.UpdateUserInfo;
using ITForum.Application.Users.Commands.UploadAvatar;
using ITForum.Application.Users.Queries.GetFullUserInfo;
using ITForum.Application.Users.Queries.GetShortUserInfo;
using ITForum.Application.Users.Queries.GetUserList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITForum.Api.Controllers
{
    [Authorize(Policy = "RequireUserRole")]
    public class UserController : BaseController
    {
        private readonly IIdentityService _identityService;

        public UserController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet]
        public async Task<ActionResult> GetUserList(UsersSort sort, int page, int pageSize)
        {
            var query = new GetUserListQuery { Sort = sort, Page = page, PageSize = pageSize, UserId = UserId };
            var users = await Mediator.Send(query);

            return Ok(users);
        }
        [HttpPut("Subscribe")]
        public async Task<ActionResult> Subscribe(Guid userId)
        {
            var command = new SubscribeOnUserCommand() { UserId = UserId, SubscribeUserId = userId};
            await Mediator.Send(command);
            return NoContent();
        }
        [HttpPut("Unsubscribe")]
        public async Task<ActionResult> Unsubscribe(Guid userId)
        {
            var command = new UnsubscribeFromUserCommand() { UserId = UserId, UnsubscribeUserId = userId };
            await Mediator.Send(command);
            return NoContent();
        }
        /// <summary>
        /// Get short user info
        /// </summary>
        /// <returns>Returns UserVm</returns>
        [HttpGet("info/{id?}")]
        public async Task<ActionResult<ShortUserInfoVm>> GetShortUserInfo(Guid? id)
        {
            if (id == null && UserId != Guid.Empty)
            {
                id = UserId;
            }
            if (id == null) throw new UnauthorizeException();
            var query = new GetShortUserInfoQuery { UserId = UserId };
            var claims = User.FindAll(identity => identity.Type == ClaimTypes.Role);
            var userInfo = await Mediator.Send(query);
            userInfo.Roles = claims.Select(x => x.Value).ToList();
            return Ok(userInfo);
        }
        /// <summary>
        /// Get full user info
        /// </summary>
        /// <returns>Returns UserVm</returns>
        [HttpGet("FullInfo/{id?}")]
        public async Task<ActionResult<FullUserInfoVm>> GetFullUserInfo([FromRoute]Guid? id)
        {
            if (id == null && UserId!=Guid.Empty)
            {
                id = UserId;
            }
            if (id == null) throw new UnauthorizeException();
            var query = new GetFullUserInfoQuery { UserId = id.Value };
            var userInfo = await Mediator.Send(query);
            return Ok(userInfo);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateUserInfo(UpdateUserInfoModel userInfo)
        {
            var command = Mapper.Map<UpdateUserInfoCommand>(userInfo);
            command.UserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
        [HttpPost("upload")]
        public async Task<ActionResult> UploadAvatar(IFormFile file)
        {
            var command = new UploadAvatarCommand()
            {
                Avatar = file,
                UserId = UserId,
                Path = GenerateAbsoluteUrl("/UploadedFiles/")
            };
            var path = await Mediator.Send(command);

            return Ok(path);
        }
        /// <summary>
        /// Delete user by Id
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete]
        public async Task<ActionResult> DeleteUser(Guid UserId)
        {
            var command = new DeleteUserCommand() { UserId = UserId };
            await Mediator.Send(command);
            return NoContent();
        }
        /// <summary>
        /// Ban user by name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("BanUser")]
        public async Task<ActionResult> BanUserByName([FromQuery]string userName)
        {
            await _identityService.BanUserByName(userName);
            return NoContent();
        }
    }
}