using ITForum.Domain.TopicItems;

namespace ITForum.Application.Topics.Queries.GetTopicDetailsByIdQuery
{
    //It's a class describing object to return for user
    public class TopicDetailsVm
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public List<Attachment> Attachment { get; set; }
    }
}
