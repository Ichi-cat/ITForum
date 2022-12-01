using System.ComponentModel.DataAnnotations;

namespace ITForum.Domain.ItForumUser
{
    public class RefreshToken
    {
        [Key]
        public Guid Token { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpiresDate { get; set; }
    }
}
