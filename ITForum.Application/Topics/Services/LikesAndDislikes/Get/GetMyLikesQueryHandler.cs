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
            var markList = await _context.Marks.Where(m => m.UserId == request.UserId).Select(m => new MarkVM
            {
                Id = m.Id,
                UserId = m.UserId,
                TopicId = m.TopicId,
                IsLiked = m.IsLiked
            }).ToListAsync();
            
            return new MarkListVM
            {
                Marks = markList
            };
        }
    }
}
