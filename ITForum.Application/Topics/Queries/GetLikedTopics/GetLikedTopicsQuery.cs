using ITForum.Application.Topics.TopicViewModels;
using MediatR;

namespace ITForum.Application.Topics.Queries.GetLikedTopics
{
    public class GetLikedTopicsQuery : IRequest<TopicListVm>
    {
        public Guid UserId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
