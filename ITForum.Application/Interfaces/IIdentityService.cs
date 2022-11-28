using ITForum.Application.Services.IdentityService;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace ITForum.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<JwtSecurityToken> CreateUserWithProvider(UserLoginInfo providerInfo, BaseUserInfoModel userInfo);
        Task<JwtSecurityToken> CreateUser(BaseUserInfoModel userInfo, string password);
        Task<JwtSecurityToken> Login(BaseUserInfoModel userInfo, string password);
        Task<JwtSecurityToken> Login(string loginProvider, string providerKey);
        Task SendToken(string email, Uri redirectUri);
        Task ResetPassword(string token, string email, string password);
    }
}
