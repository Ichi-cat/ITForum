using MediatR;

namespace ITForum.Application.Topic.Comands.CreateTopicCommand
{
    public class CreateTopicCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
