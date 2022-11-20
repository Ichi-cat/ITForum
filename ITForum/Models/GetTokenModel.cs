namespace ITForum.Api.Models
{
    public class GetTokenModel
    {
        public string Email { get; set; } = null!;
        public Uri RedirectUri { get; set; } = null!;
    }
}
