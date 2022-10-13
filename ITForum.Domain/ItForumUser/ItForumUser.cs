using ITForum.Domain.TopicItems;
using Microsoft.AspNetCore.Identity;

namespace ITForum.Domain.ItForumUser
{
    public class ItForumUser : IdentityUser<Guid>
    {
        public List<Mark> Marks { get; set; }
    }
}
