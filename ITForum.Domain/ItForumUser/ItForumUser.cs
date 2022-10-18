using Microsoft.AspNetCore.Identity;

namespace ITForum.Domain.ItForumUser
{
    public class ItForumUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = "NoName";
        public string LastName { get; set; } = String.Empty;
        public string Avatar { get; set; } = "https://www.gravatar.com/avatar/00000000000000000000000000000000?d=mp&f=y";
        public string Description { get; set; } = "No information";

        public string FullName => $"{FirstName} {LastName}";
    }
}
