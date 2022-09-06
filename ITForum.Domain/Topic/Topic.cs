namespace ITForum.Domain.Topic
{
    public class Topic
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public List<Attachment> Attachment { get; set; }
        public List<Comment> Comment { get; set; }
    }
}
