using ITForum.Application.Common.Exceptions;
using ITForum.Application.Interfaces;
using ITForum.Domain.TopicItems;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Topics.Services.LikesAndDislikes
{
    public class LikeCommandHandler : IRequestHandler<LikeCommand>
    {
        private readonly IItForumDbContext _context;
        public LikeCommandHandler(IItForumDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(LikeCommand request, CancellationToken cancellationToken)
        {
            var entityMark = await _context.Marks.FirstOrDefaultAsync(mark =>
            mark.UserId == request.UserId && mark.TopicId == request.TopicId, cancellationToken);
            // TODO: проверить существование топика
            if (entityMark == null )
            {
                var mark = new Mark
                {
                    Id = Guid.NewGuid(),
                    isLiked = request.IsLiked,
                    TopicId = request.TopicId
                };
                await _context.Marks.AddAsync(mark);
            }
            else
            {
                if (entityMark.isLiked == request.IsLiked)
                {
                    _context.Marks.Remove(entityMark);
                }
                else
                {
                    entityMark.isLiked = request.IsLiked;
                }
            }
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
