namespace ITForum.Domain.TopicItems
{
    public class Mark
    {
        public Guid Id { get; set; }
        public MarkType Type { get; set; }
        public Guid UserId { get; set; }
    }
    public enum MarkType
    {
        LIKE,
        DISLIKE
    }
}
