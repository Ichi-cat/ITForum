namespace ITForum.Application.Services.IdentityService
{
    public class AdditionalUserInformation
    {
        public AdditionalUserInformation(string email, string username)
        {
            Email = email;
            UserName = username;
        }
        public string Email { get; init; }
        public string UserName { get; init; }
    }
}
