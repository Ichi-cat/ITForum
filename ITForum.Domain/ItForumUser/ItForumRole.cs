using Microsoft.AspNetCore.Identity;

namespace ITForum.Domain.ItForumUser
{
    public class ItForumRole : IdentityRole<Guid>
    {
        public ItForumRole(string name) : base(name) { }
    }
}
