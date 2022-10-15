namespace ITForum.Domain.TopicItems
{
    public class Attachment
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public string Url { get; set; }
        public Guid? TopicId { get; set; }
    }
}
                                                                                                          