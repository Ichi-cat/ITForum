namespace ITForum.Api.Models.Auth
{
    public class TokenVm
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
