namespace ITForum.Domain.TopicItems
{
    public class Topic
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public List<Attachment> Attachments { get; set; }
        public List<Comment> Comment { get; set; }
        public DateTime Created { get; set; }
        public DateTime? EditDate { get; set; }
        public List<Mark> Marks { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
