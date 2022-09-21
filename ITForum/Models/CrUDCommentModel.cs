using ITForum.Domain.TopicItems;

namespace ITForum.Api.Models
{
    public class CrUDCommentModel
    {
        public string Content { get; set; }
        public Topic? Topic { get; set; }
        public Comment? Comm { get; set; }
    }
}
