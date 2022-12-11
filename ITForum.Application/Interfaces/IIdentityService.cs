using ITForum.Application.Common.ViewModels;
using ITForum.Application.Services.IdentityService;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace ITForum.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<TokenVm> CreateUserWithProvider(UserLoginInfo providerInfo, BaseUserInfoModel userInfo);
        Task<TokenVm> CreateUser(BaseUserInfoModel userInfo, string password);
        Task<TokenVm> Login(BaseUserInfoModel userInfo, string password);
        Task<TokenVm> Login(string loginProvider, string providerKey);
        Task<TokenVm> RefreshToken(Guid refreshToken, string accessToken);
        Task SendToken(string email, Uri redirectUri);
        Task ResetPassword(string token, string email, string password);
        Task ChangePassword(Guid userId, string oldPassword, string newPassword);
        Task ChangeEmail(string email, Guid userId);
        Task BanUserByName(string userName);
    }
}
