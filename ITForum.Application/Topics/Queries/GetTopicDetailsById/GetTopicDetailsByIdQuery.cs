using MediatR;
using ITForum.Domain.TopicItems;

namespace ITForum.Application.Topics.Queries.GetTopicDetailsById
{
    public class GetTopicDetailsByIdQuery : IRequest<TopicDetailsVm>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
