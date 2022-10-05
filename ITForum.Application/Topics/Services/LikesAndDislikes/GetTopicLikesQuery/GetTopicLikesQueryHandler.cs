﻿using ITForum.Application.Interfaces;
using MediatR;
using ITForum.Domain.TopicItems;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Topics.Services.LikesAndDislikes.GetTopicLikesQuery
{
    public class GetTopicLikesQueryHandler : IRequestHandler<GetTopicLikesQuery, int>
    {
        private readonly IItForumDbContext _context;
        public GetTopicLikesQueryHandler(IItForumDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(GetTopicLikesQuery request, CancellationToken cancellationToken)
        {
            var marks = await _context.Marks.Where(m => m.UserId == request.UserId && m.IsLiked == MarkType.LIKE).CountAsync();
            return marks;
        }
    }
}
