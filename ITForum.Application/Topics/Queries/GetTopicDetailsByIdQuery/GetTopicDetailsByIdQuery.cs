using MediatR;
using ITForum.Domain.TopicItems;

namespace ITForum.Application.Topics.Queries.GetTopicDetailsByIdQuery
{
    public class GetTopicDetailsByIdQuery : IRequest<Topic>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
