using ITForum.Application.Interfaces;
using ITForum.Application.Tags.Commands.CreateTag;
using ITForum.Domain.TopicItems;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Topics.Commands.CreateTopic
{
    /// <summary>
    /// Логика создания топика
    /// </summary>
    public class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, Guid>
    {
        private readonly IItForumDbContext _context;
        private readonly IMediator _mediator;

        public CreateTopicCommandHandler(IItForumDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        public async Task<Guid> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = new Topic
            {
                Name = request.Name,
                Content = request.Content,
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Created = DateTime.Now
            };

            
            var topicTags = await _mediator.Send(new CreateTagCommand { TagNames = request.Tags });
            topic.Tags = topicTags;

            await _context.Attachments.Where(attachment => request.AttachmentsId
                .Contains(attachment.Id))
                    .ForEachAsync(attachment => attachment.TopicId = topic.Id);
            

            await _context.Topics.AddAsync(topic);
            await _context.SaveChangesAsync();
            return topic.Id;
        }
    }
}
