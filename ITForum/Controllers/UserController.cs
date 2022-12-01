using ITForum.Api.enums;
using ITForum.Api.Models;
using ITForum.Application.Common.Exceptions;
using ITForum.Application.Users.Commands.SubscribeOnUser;
using ITForum.Application.Users.Commands.UnsubscribeFromUser;
using ITForum.Application.Users.Commands.UpdateUserInfo;
using ITForum.Application.Users.Commands.UploadAvatar;
using ITForum.Application.Users.Queries.GetFullUserInfo;
using ITForum.Application.Users.Queries.GetShortUserInfo;
using ITForum.Application.Users.Queries.GetUserList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITForum.Api.Controllers
{
    public class UserController : BaseController
    {
        [AllowAnonymous]
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
        /// Get user info
        /// </summary>
        /// <returns>Returns UserVm</returns>
        [AllowAnonymous]
        [HttpGet("info/{id?}")]
        public async Task<ActionResult<ShortUserInfoVm>> GetShortUserInfo(Guid? id)
        {
            if (id == null && UserId != Guid.Empty)
            {
                id = UserId;
            }
            if (id == null) throw new AuthenticationError(new[] { "User not found" });
            var query = new GetShortUserInfoQuery { UserId = UserId };
            var userInfo = await Mediator.Send(query);
            return Ok(userInfo);
        }
        /// <summary>
        /// Get full user info
        /// </summary>
        /// <returns>Returns UserVm</returns>
        [AllowAnonymous]
        [HttpGet("FullInfo/{id?}")]
        public async Task<ActionResult<FullUserInfoVm>> GetFullUserInfo([FromRoute]Guid? id)
        {
            if (id == null && UserId!=Guid.Empty)
            {
                id = UserId;
            }
            if (id == null) throw new AuthenticationError(new[] { "User not found" });
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
    }
}