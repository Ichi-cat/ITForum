﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITForum.Api.enums;
using ITForum.Api.Models;
using ITForum.Api.ViewModels;
using ITForum.Application.Common.Exceptions;
using ITForum.Application.Interfaces;
using ITForum.Domain.ItForumUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using System.Collections.Generic;
using System.Diagnostics;

namespace ITForum.Api.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserManager<ItForumUser> _userManager;
        private readonly RoleManager<ItForumRole> _roleManager;
        private readonly IBufferedFileUploadService _uploadFile;
        private readonly IMapper _mapper;

        public UserController(UserManager<ItForumUser> userManager, RoleManager<ItForumRole> roleManager,
            IBufferedFileUploadService uploadFile,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _uploadFile = uploadFile;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetUserList(UsersSort sort, int page, int pageSize)
        {
            List<ItForumUser> subscriptions;
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                subscriptions = new List<ItForumUser>();
            }
            else
            {
                subscriptions = await _userManager.Users.Where(u => u.Subscribers.Any(u => u.Id == UserId)).ToListAsync();
            }
            var query = _userManager.Users
                .Include(u => u.Subscriptions)
                .ProjectTo<UserItem>(_mapper.ConfigurationProvider);

            switch (sort)
            {
                case UsersSort.ByNameAsc:
                    query = query.OrderBy(u => u.FirstName);
                    break;
                case UsersSort.ByNameDesc:
                    query = query.OrderByDescending(u => u.FirstName);
                    break;
                default:
                    break;
            }
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            
            int count = await _userManager.Users.CountAsync();
            int pagesCount = (int)Math.Ceiling(((decimal)count)/pageSize);

            var users = await query.ToListAsync();
            users.ForEach(u => u.IsSubscribed = subscriptions.Any(s => s.Id == u.Id));

            return Ok(new { users = users, pagesCount = pagesCount});
        }
        [HttpPut("Subscribe")]
        public async Task<ActionResult> Subscribe(Guid userId)
        {
            var user = await _userManager.Users.Include(u => u.Subscriptions).FirstOrDefaultAsync(u => u.Id == UserId);
            var userSub = await _userManager.FindByIdAsync(userId.ToString());
            if (userSub == null)
            {
                throw new Exception("User not found");
            }
            user?.Subscriptions.Add(userSub);
            if(user!=null) await _userManager.UpdateAsync(user);
            return NoContent();
        }
        [HttpPut("Unsubscribe")]
        public async Task<ActionResult> Unsubscribe(Guid userId)
        {
            var user = await _userManager.Users.Include(u => u.Subscriptions).FirstOrDefaultAsync(u => u.Id == UserId);
            var userSub = await _userManager.FindByIdAsync(userId.ToString());
            if (userSub == null)
            {
                throw new Exception("User not found");
            }
            user?.Subscriptions.Remove(userSub);
            if (user != null) await _userManager.UpdateAsync(user);
            return NoContent();
        }
        /// <summary>
        /// Get user info
        /// </summary>
        /// <returns>Returns UserVm</returns>
        [HttpGet("info")]
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
        [HttpGet("FullInfo/{id?}")]
        public async Task<ActionResult<FullUserInfoVm>> GetFullUserInfo([FromRoute]Guid? id)
        {
            if (id == null)
            {
                id = UserId;
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            var userInfo = Mapper.Map<FullUserInfoVm>(user);
            return userInfo;
        }
        [HttpPut]
        public async Task<ActionResult> UpdateUserInfo(UpdateUserInfoModel userInfo)
        {
            ItForumUser user = await _userManager.FindByIdAsync(UserId.ToString());

            user.FirstName = userInfo.FirstName ?? user.FirstName;
            user.LastName = userInfo.LastName ?? user.LastName;
            user.Description = userInfo.Description ?? user.Description;
            user.Avatar = userInfo.Avatar ?? user.Avatar;
            user.Location = userInfo.Location ?? user.Location;
            user.BirthDate = userInfo.BirthDate ?? user.BirthDate;
            user.Study = userInfo.Study ?? user.Study;
            user.Work = userInfo.Work ?? user.Work;
            await _userManager.UpdateAsync(user);
            return Ok();
        }
        [HttpPost("upload")]
        public async Task<ActionResult> UploadAvatar(IFormFile file)
        {
            if (file.ContentType != "image/jpeg" 
                && file.ContentType != "image/png")
            {
                throw new UploadFileException("File type is not supported");
            }
            if ((file.Length / 1024 / 1024) > 5) throw new UploadFileException("Max size is 5MB");
            using (var stream = file.OpenReadStream())
            {
                using(var image = Image.Load(stream))
                {
                    if (image.Height != image.Width) throw new UploadFileException("Height must equal width");
                }
            }
            var extension = Path.GetExtension(file.FileName);
            if (extension == null || extension == "")
            {
                extension = ".jpg";
            }
            var path = "/UploadedFiles/" + (await _uploadFile.UploadFile(file, Guid.NewGuid().ToString() + extension));
            path = GenerateAbsoluteUrl(path);
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            user.Avatar = path;
            await _userManager.UpdateAsync(user);
            return Ok(path);
        }
    }
}