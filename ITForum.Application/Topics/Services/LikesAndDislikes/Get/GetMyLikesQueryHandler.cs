using ITForum.Application.Interfaces;
using ITForum.Application.Topics.Queries.GetMyTopicListCommand;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ITForum.Domain.TopicItems.Mark.MarkType;

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
            var marks = _context.Marks.Where(m => m.UserId == request.UserId && m.IsLiked == LIKE).ToList();
            
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
