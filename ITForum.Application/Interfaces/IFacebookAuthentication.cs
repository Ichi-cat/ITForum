using ITForum.Application.Common.JsonModels;

namespace ITForum.Application.Interfaces
{
    public interface IFacebookAuthentication
    {
        public Task<FacebookTokenValidation> ValidateToken(string accessToken);
        public Task<FacebookUserInformation> GetUserInformation(string accessToken);
    }
}
