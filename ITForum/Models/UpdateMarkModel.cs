using ITForum.Domain.TopicItems;
using static ITForum.Domain.TopicItems.Mark;

namespace ITForum.Api.Models
{
    public class UpdateMarkModel
    {
        public Guid TopicId { get; set; }
        public MarkType IsLiked { get; set; }
    }
}
