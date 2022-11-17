using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITForum.Domain.ItForumUser
{
    public class ItForumUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string? Avatar { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? Location { get; set; } = null;
        public string? BirthLocation { get; set; } = null;
        public DateTime? BirthDate { get; set; } = null;
        public string? Study { get; set; } = null;
        public string? Work { get; set; } = null;
        public string? TimeZone { get; set; } = null;

        public List<ItForumUser> Subscriptions { get; set; } = null!;
        public List<ItForumUser> Subscribers { get; set; } = null!;

        public string FullName => $"{FirstName} {LastName}";
    }
}
