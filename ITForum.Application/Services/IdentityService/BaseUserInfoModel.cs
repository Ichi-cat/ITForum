namespace ITForum.Application.Services.IdentityService
{
    public class BaseUserInfoModel
    {
        public bool IsEmailConfirmed { get; set; } = false;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
    }
}
