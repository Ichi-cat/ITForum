namespace ITForum.Domain.TopicItems
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public Guid? TopicId { get; set; } 
        public Guid? CommId { get; set; }
        public List<Mark> Marks { get; set; }
    }
}
