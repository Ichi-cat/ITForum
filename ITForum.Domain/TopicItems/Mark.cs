namespace ITForum.Domain.TopicItems
{
    public class Mark
    {
        public Guid Id { get; set; }
        public Guid TopicId { get; set; }
        public MarkType IsLiked { get; set; }
        public Guid UserId { get; set; }

        public enum MarkType
        {
            LIKE,
            DISLIKE, 
        }
    }
}
