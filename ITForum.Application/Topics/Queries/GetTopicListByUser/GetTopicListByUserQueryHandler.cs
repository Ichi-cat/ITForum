using ITForum.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ITForum.Application.Topics.TopicViewModels;
using ITForum.Application.Common.Extensions;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace ITForum.Application.Topics.Queries.GetTopicListByUser
{
    public class GetTopicListByUserQueryHandler : IRequestHandler<GetTopicListByUserQuery, TopicListVm>
    {
        private readonly IItForumDbContext _context;
        private readonly IMapper _mapper;
        public GetTopicListByUserQueryHandler(IItForumDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TopicListVm> Handle(GetTopicListByUserQuery request, CancellationToken cancellationToken)
        {
            var topics = await _context.Topics
                .Where(topic => topic.UserId == request.UserId)
                .ProjectTo<TopicVm>(_mapper.ConfigurationProvider)
                .Sort(request.Sort)
                .Paginate(request.Page, request.PageSize)
                .ToListAsync(cancellationToken);

            int pageCount = await _context.Topics
                .Where(topic => topic.UserId == request.UserId)
                .GetPageCount(request.PageSize);

            return new TopicListVm { Topics = topics, PageCount = pageCount };
        }
    }
}
