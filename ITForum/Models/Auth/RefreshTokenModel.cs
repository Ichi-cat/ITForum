namespace ITForum.Api.Models.Auth
{
    public class RefreshTokenModel
    {
        public Guid RefreshToken { get; set; }
        public string AccessToken { get; set; } = null!;
    }
}
