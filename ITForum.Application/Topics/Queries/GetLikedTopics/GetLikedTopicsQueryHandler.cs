using AutoMapper;
using ITForum.Application.Interfaces;
using ITForum.Application.Topics.Queries.GetMyTopicList;
using ITForum.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Topics.Queries.GetLikedTopics
{
    public class GetLikedTopicsQueryHandler : IRequestHandler<GetLikedTopicsQuery, LikedTopicsListVm>
    {
        private readonly IItForumDbContext _context;
        private readonly IMapper _mapper;
        public GetLikedTopicsQueryHandler(IItForumDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<LikedTopicsListVm> Handle(GetLikedTopicsQuery request, CancellationToken cancellationToken)
        {
            var userLikedMarks = await _context.Marks
                .Where(x => x.UserId == request.UserId && x.IsLiked == MarkType.LIKE).ToListAsync(cancellationToken);
                
            var likedTopicsVm = new List<LikedTopicVm>();
            
            foreach (var mark in userLikedMarks)
            {
                var topic = await _context.Topics.FindAsync(mark.TopicId);
                var topicVm = new LikedTopicVm();
                topicVm.TopicId = topic.Id;
                topicVm.Name = topic.Name;
                topicVm.ShortContent = topic.Content.Length > 200 ? topic.Content.Substring(0, 200) : topic.Content;
                likedTopicsVm.Add(topicVm);
            }
            

            return new LikedTopicsListVm { LikedTopics = likedTopicsVm };
        }
    }
}
