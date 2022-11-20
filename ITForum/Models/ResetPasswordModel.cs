namespace ITForum.Api.Models
{
    public class ResetPasswordModel
    {
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!; 
    }
}
