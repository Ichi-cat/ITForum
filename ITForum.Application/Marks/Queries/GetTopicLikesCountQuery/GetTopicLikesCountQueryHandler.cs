using ITForum.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ITForum.Domain.Enums;

namespace ITForum.Application.Marks.Queries.GetTopicLikesCountQuery
{
    public class GetTopicLikesCountQueryHandler : IRequestHandler<GetTopicLikesCountQuery, int>
    {
        private readonly IItForumDbContext _context;
        public GetTopicLikesCountQueryHandler(IItForumDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(GetTopicLikesCountQuery request, CancellationToken cancellationToken)
        {
            // TODO: не правильно работает
            var marks = await _context.Marks.Where(m => m.UserId == request.UserId && m.IsLiked == MarkType.LIKE).CountAsync();
            return marks;
        }
    }
}
