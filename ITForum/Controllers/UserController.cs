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
        [HttpGet]
        public async Task<ActionResult<UserInfoVm>> GetUserInfo()
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            return new UserInfoVm
            {
                UserName = user.UserName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user)
            };
        }
    }
}