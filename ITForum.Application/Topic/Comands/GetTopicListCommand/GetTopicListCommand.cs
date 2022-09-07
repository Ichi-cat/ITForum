using MediatR;

namespace ITForum.Application.Topic.Comands.GetTopicListCommand
{
    public class GetTopicListCommand : IRequest<TopicListVm>
    {
        public Guid UserId { get; set; }
    }
}
