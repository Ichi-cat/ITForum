namespace ITForum.Api.Models
{
    public class CreateTopicModel
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public List<Guid>? AttachmentsId { get; set; }
        public List<string>? TagsNames { get; set; }
    }
}
