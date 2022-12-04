using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITForum.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ITForum.Domain.Enums;
using ITForum.Application.Topics.TopicViewModels;
using ITForum.Application.Common.Extensions;

namespace ITForum.Application.Topics.Queries.GetTopicList
{
    public class GetTopicListQueryHandler : IRequestHandler<GetTopicListQuery, TopicListVm>
    {
        private readonly IItForumDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetTopicListQueryHandler(IItForumDbContext dbContext,
            IMapper mapper) =>
            (_dbContext, _mapper) = (dbContext, mapper);
        public async Task<TopicListVm> Handle(GetTopicListQuery request, CancellationToken cancellationToken)
        {
            var topicQuery = _dbContext.Topics
                .Include(topic => topic.Marks)
                .ProjectTo<TopicVm>(_mapper.ConfigurationProvider)
                .Sort(request.Sort);

            int pageCount = await _dbContext.Topics.GetPageCount(request.PageSize);
            topicQuery =  topicQuery.Paginate(request.Page, request.PageSize);

            return new TopicListVm { Topics = await topicQuery.ToListAsync(cancellationToken), PageCount = pageCount };
        }
    }
}
