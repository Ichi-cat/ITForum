using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            var likedTopics = await _context.Marks.Include(mark => mark.Topic).Where(mark => mark.UserId == request.UserId && mark.IsLiked == MarkType.LIKE)
                .Select(mark => mark.Topic)
                .ProjectTo<LikedTopicVm>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new LikedTopicsListVm { LikedTopics = likedTopics };
        }
    }
}
