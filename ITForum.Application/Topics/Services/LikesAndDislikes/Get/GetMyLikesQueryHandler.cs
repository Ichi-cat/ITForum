using ITForum.Application.Interfaces;
using MediatR;
using ITForum.Domain.TopicItems;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Topics.Services.LikesAndDislikes.Get
{
    public class GetMyLikesQueryHandler : IRequestHandler<GetMyLikesQuery, MarkListVM>
    {
        private readonly IItForumDbContext _context;
        public GetMyLikesQueryHandler(IItForumDbContext context)
        {
            _context = context;
        }
        public async Task<MarkListVM> Handle(GetMyLikesQuery request, CancellationToken cancellationToken)
        {
            var marks = await _context.Marks.Where(m => m.UserId == request.UserId && m.IsLiked == MarkType.LIKE).ToListAsync();
            
            var markList = new MarkListVM();
            if (markList != null)
            {
                markList.Marks = marks.Select(m => new MarkVM
                {
                    Id = m.Id,
                    TopicId = m.TopicId,
                }).ToList();
            }
            return markList;
        }
    }
}
