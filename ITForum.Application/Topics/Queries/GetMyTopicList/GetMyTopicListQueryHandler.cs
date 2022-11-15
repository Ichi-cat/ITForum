using ITForum.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ITForum.Application.Topics.TopicViewModels;
using ITForum.Application.Common.Extensions;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace ITForum.Application.Topics.Queries.GetMyTopicList
{
    public class GetMyTopicListQueryHandler : IRequestHandler<GetMyTopicListQuery, TopicListVm>
    {
        private readonly IItForumDbContext _context;
        private readonly IMapper _mapper;
        public GetMyTopicListQueryHandler(IItForumDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TopicListVm> Handle(GetMyTopicListQuery request, CancellationToken cancellationToken)
        {
            var topics = await _context.Topics
                .Where(topic => topic.UserId == request.UserId)
                .Paginate(request.Page, request.PageSize)
                .ProjectTo<TopicVm>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            int pageCount = await _context.Topics.GetPageCount(request.PageSize);

            return new TopicListVm { Topics = topics, PageCount = pageCount };
        }
    }
}
