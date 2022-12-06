using ITForum.Application.Tags.TagsViewModel;
using MediatR;

namespace ITForum.Application.Tags.Queries.GetTopicTags
{
    public class GetTopicTagsQuery : IRequest<List<string>>
    {
        public Guid TopicId { get; set; }
    }
}
