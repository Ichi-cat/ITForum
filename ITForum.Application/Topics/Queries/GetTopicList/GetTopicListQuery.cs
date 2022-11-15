using ITForum.Application.Topics.TopicViewModels;
using ITForum.Domain.Enums;
using ITForum.Domain.TopicItems;
using MediatR;

namespace ITForum.Application.Topics.Queries.GetTopicList
{
    public class GetTopicListQuery : IRequest<TopicListVm>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public TypeOfSort Sort { get; set; }
    }
}
