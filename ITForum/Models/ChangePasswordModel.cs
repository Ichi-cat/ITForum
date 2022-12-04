namespace ITForum.Api.Models
{
    public class ChangePasswordModel
    {
        public string? OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
