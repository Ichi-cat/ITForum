using MediatR;
using ITForum.Application.Topics.TopicViewModels;
using System.Text;

namespace ITForum.Application.Topics.Queries.GetMyTopicList
{
    public class GetMyTopicListQuery : IRequest<TopicListVm>
    {
        public Guid UserId { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
