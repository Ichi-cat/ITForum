using ITForum.Application.Interfaces;
using ITForum.Application.Topics.Queries.GetMyTopicListCommand;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Topics.Services.LikesAndDislikes.Get
{
    internal class GetMyLikesQueryHandler : IRequestHandler<GetMyLikesQuery, MarkListVM>
    {
        private readonly IItForumDbContext _context;
        public GetMyLikesQueryHandler(IItForumDbContext context)
        {
            _context = context;
        }
        public async Task<MarkListVM> Handle(GetMyLikesQuery request, CancellationToken cancellationToken)
        {
            var marks = _context.Marks.Where(m => m.UserId == request.UserId).ToList();
            var markList = new MarkListVM();
            markList.Marks = marks.Select(m => new MarkVM
            {
                Id = m.Id,
                UserId = m.UserId,
                TopicId = m.TopicId,
                IsLiked = m.IsLiked
            }).ToList();
            return markList;
        }
    }
}
