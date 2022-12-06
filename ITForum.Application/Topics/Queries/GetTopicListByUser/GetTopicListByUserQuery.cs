using MediatR;
using ITForum.Application.Topics.TopicViewModels;
using ITForum.Domain.Enums;

namespace ITForum.Application.Topics.Queries.GetTopicListByUser
{
    public class GetTopicListByUserQuery : IRequest<TopicListVm>
    {
        public Guid? UserId { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public TypeOfSort Sort { get; set; }
    }
}
