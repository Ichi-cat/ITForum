using Microsoft.AspNetCore.Identity;

namespace ITForum.Persistance.TempEntities
{
    public class ItForumRole : IdentityRole<Guid>
    {
        public ItForumRole(string name) : base(name){}
    }
}
