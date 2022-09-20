using ITForum.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using ITForum.Domain.TopicItems;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITForum.Application.Common.Exceptions;

namespace ITForum.Application.Comments.Commands.UpdateComment
{
    internal class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand>
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
            entity.Topic = request.Topic;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
