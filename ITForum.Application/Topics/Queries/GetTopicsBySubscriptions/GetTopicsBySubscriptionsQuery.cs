using ITForum.Application.Topics.TopicViewModels;
using ITForum.Domain.Enums;
using MediatR;

namespace ITForum.Application.Topics.Queries.GetTopicsBySubscriptions
{
    public class GetTopicsBySubscriptionsQuery : IRequest<TopicListVm>
    {
        public Guid UserId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public TypeOfSort Sort { get; set; }
    }
}
