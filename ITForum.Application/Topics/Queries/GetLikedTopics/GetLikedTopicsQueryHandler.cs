using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITForum.Application.Common.Extensions;
using ITForum.Application.Interfaces;
using ITForum.Application.Topics.TopicViewModels;
using ITForum.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Topics.Queries.GetLikedTopics
{
    public class GetLikedTopicsQueryHandler : IRequestHandler<GetLikedTopicsQuery, TopicListVm>
    {
        private readonly IItForumDbContext _context;
        private readonly IMapper _mapper;
        public GetLikedTopicsQueryHandler(IItForumDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TopicListVm> Handle(GetLikedTopicsQuery request, CancellationToken cancellationToken)
        {
            var likedTopics = await _context.Marks
                .Include(mark => mark.Topic)
                .Where(mark => mark.UserId == request.UserId && mark.IsLiked == MarkType.LIKE)
                .Select(mark => mark.Topic)
                .Paginate(request.Page, request.PageSize)
                .ProjectTo<TopicVm>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            
            int pageCount = await _context.Marks
                .Include(mark => mark.Topic)
                .Where(mark => mark.UserId == request.UserId && mark.IsLiked == MarkType.LIKE)
                .Select(mark => mark.Topic)
                .GetPageCount(request.PageSize);

            return new TopicListVm { Topics = likedTopics, PageCount = pageCount};
        }
    }
}
