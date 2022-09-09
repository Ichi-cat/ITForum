using ITForum.Domain.TopicItems;
using MediatR;

namespace ITForum.Application.Topics.Comands.CreateTopicCommand
{
    public class CreateTopicCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}
