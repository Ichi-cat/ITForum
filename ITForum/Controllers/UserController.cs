using ITForum.Api.Models;
using ITForum.Api.ViewModels;
using ITForum.Domain.ItForumUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITForum.Api.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserManager<ItForumUser> _userManager;
        private readonly RoleManager<ItForumRole> _roleManager;

        public UserController(UserManager<ItForumUser> userManager, RoleManager<ItForumRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        /// <summary>
        /// Get user info
        /// </summary>
        /// <returns>Returns UserVm</returns>
        [HttpGet()]
        public async Task<ActionResult<UserInfoVm>> GetShortUserInfo()
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            var userInfo = Mapper.Map<UserInfoVm>(user);
            userInfo.Roles = await _userManager.GetRolesAsync(user);
            return userInfo;
        }
        /// <summary>
        /// Get full user info
        /// </summary>
        /// <returns>Returns UserVm</returns>
        [HttpGet("FullInfo")]
        public async Task<ActionResult<FullUserInfoVm>> GetFullUserInfo()
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            var userInfo = Mapper.Map<FullUserInfoVm>(user);
            return userInfo;
        }
        [HttpPut]
        public async Task<ActionResult> UpdateUserInfo(UpdateUserInfoModel userInfo)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            user.FirstName = userInfo.FirstName;
            user.LastName = userInfo.LastName;
            user.Description = userInfo.Description;
            await _userManager.UpdateAsync(user);
            return Ok();
        }
    }
}