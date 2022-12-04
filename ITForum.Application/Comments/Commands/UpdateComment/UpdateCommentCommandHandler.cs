using ITForum.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ITForum.Domain.TopicItems;
using ITForum.Application.Common.Exceptions;

namespace ITForum.Application.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand>
    {
        private readonly IItForumDbContext _context;
        public UpdateCommentCommandHandler(IItForumDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Comments.FirstOrDefaultAsync(comment =>
            comment.Id == request.Id, cancellationToken);
            if (entity==null||entity.UserId!=request.UserId)
            {
                throw new NotFoundException(nameof(Comment), request.Id);
            }
            entity.Content = request.Content;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
