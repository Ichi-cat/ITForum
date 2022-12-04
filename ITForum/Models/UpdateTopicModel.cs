namespace ITForum.Api.Models
{
    public class UpdateTopicModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public List<Guid> AttachmentsId { get; set; }
    }
}
