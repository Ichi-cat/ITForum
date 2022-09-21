using System.ComponentModel.DataAnnotations;

namespace ITForum.Api.Models.Auth
{
    public class SignInModel
    {
        [Required]
        public string UserName { get; init; }
        [Required]
        public string Password { get; init; }
    }
}
