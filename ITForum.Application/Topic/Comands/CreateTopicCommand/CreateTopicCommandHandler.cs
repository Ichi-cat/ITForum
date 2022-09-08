using MediatR;

namespace ITForum.Application.Topic.Comands.CreateTopicCommand
{
    internal class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, Guid>
    {
        public async Task<Guid> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            return Guid.NewGuid();
        }
    }
}
