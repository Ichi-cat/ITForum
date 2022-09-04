namespace ITForum.Domain.Topic
{
    internal class Mark
    {
        public Guid Id { get; set; }
        public MarkType Type { get; set; }
        public Guid UserId { get; set; }
    }
    enum MarkType
    {
        LIKE,
        DISLIKE
    }
}
