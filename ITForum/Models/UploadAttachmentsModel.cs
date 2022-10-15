namespace ITForum.Api.Models
{
    public class UploadAttachmentsModel
    {
        public IFormFile Attachment { get; set; }
        public Guid TopicId { get; set; }
    }
}
