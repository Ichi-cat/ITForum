using MediatR;
using ITForum.Application.Interfaces;
using ITForum.Application.Common.Exceptions;
using ITForum.Domain.TopicItems;

namespace ITForum.Application.Topics.Comands.DeleteTopic
{
    internal class DeleteTopicCommandHandler
        : IRequestHandler<DeleteTopicCommand>
    {
        private readonly IItForumDbContext _context;
        public DeleteTopicCommandHandler(IItForumDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Topics.FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Topic), request.Id);
            }

            _context.Topics.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
