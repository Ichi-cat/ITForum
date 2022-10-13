using ITForum.Domain.Enums;

namespace ITForum.Api.Models
{
    public class UpdateMarkModel
    {
        public Guid TopicId { get; set; }
        public MarkType IsLiked { get; set; }
    }
}
