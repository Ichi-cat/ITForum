using MediatR;

namespace ITForum.Application.Topic.Comands.GetTopicListCommand
{
    internal class GetTopicListCommandHandler : IRequestHandler<GetTopicListCommand, TopicListVm>
    {
        public async Task<TopicListVm> Handle(GetTopicListCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
