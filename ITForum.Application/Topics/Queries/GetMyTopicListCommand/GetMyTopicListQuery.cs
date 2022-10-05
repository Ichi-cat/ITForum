using MediatR;

namespace ITForum.Application.Topics.Queries.GetMyTopicListCommand
{
    public class GetMyTopicListQuery : IRequest<TopicListVm>
    {
        public Guid UserId { get; set; }    
    }
}
