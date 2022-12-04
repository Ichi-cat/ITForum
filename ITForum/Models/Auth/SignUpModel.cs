using System.ComponentModel.DataAnnotations;

namespace ITForum.Api.Models.Auth
{
    public class SignUpModel
        //todo: add normal validation
    {
        [Required]
        public string UserName { get; init; }
        [Required]
        public string Password { get; init; }
        [Compare("Password")]
        public string ConfirmPassword { get; init; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; init; }
    }
}
