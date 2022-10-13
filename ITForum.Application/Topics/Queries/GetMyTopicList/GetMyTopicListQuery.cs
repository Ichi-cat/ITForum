using MediatR;

namespace ITForum.Application.Topics.Queries.GetMyTopicList
{
    public class GetMyTopicListQuery : IRequest<TopicListVm>
    {
        public Guid UserId { get; set; }
    }
}
