using MediatR;
using Microsoft.EntityFrameworkCore;
using ITForum.Application.Interfaces;
using ITForum.Application.Common.Exceptions;
using ITForum.Domain.TopicItems;

namespace ITForum.Application.Topics.Commands.UpdateTopic
{
    /// <summary>
    /// Логика обновления топика
    /// </summary>
    public class UpdateTopicCommandHandler
        : IRequestHandler<UpdateTopicCommand>
    {
        private readonly IItForumDbContext _context;
        public UpdateTopicCommandHandler(IItForumDbContext context)
        {
            _context = context; 
        }
        public async Task<Unit> Handle(UpdateTopicCommand request, 
            CancellationToken cancellationToken)
        {
            var entity = await _context.Topics.FirstOrDefaultAsync(topic =>
            topic.Id == request.Id, cancellationToken);

            if (entity == null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Topic), request.Id);
            }

            entity.Name = request.Name;
            entity.Content = request.Content;
            entity.EditDate = DateTime.Now;
            entity.AttachmentsId = request.AttachmentsId; // not required field

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
