namespace ITForum.Api.ViewModels
{
    public class UserInfoVm
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
