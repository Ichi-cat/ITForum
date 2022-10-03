namespace ITForum.Api.Models
{
    public class CreateCommentModel
    {
        public Guid CommId { get; set; }
        public Guid TopicId { get; set; }
        public string Content { get; set; }
    }
}
