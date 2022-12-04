using ITForum.Application.Common.JsonModels;
using System.Collections.Generic;

namespace ITForum.Application.Interfaces
{
    public interface IGitHubAuthentication
    {
        Task<AccessToken> GetAccessToken(string code);
        Task<GitHubUserInfo> GetUserInformation(string accessToken);
        Task<IEnumerable<GitHubUserEmail>> GetUserEmails(string accessToken);
    }
}
