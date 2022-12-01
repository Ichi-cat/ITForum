namespace ITForum.Application.Common.ViewModels
{
    public class TokenVm
    {
        public string AccessToken { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public Guid RefreshToken { get; set; }
    }
}
