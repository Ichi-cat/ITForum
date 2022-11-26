using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITForum.Application.Interfaces;
using ITForum.Application.Topics.TopicViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ITForum.Application.Common.Extensions;
using ITForum.Domain.Enums;

namespace ITForum.Application.Topics.Queries.GetTopicListByTag
{
    public class GetTopicListByTagQueryHandler : IRequestHandler<GetTopicListByTagQuery, TopicListVm>
    {
        private readonly IItForumDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetTopicListByTagQueryHandler(IItForumDbContext dbContext,
            IMapper mapper) =>
            (_dbContext, _mapper) = (dbContext, mapper);
        public async Task<TopicListVm> Handle(GetTopicListByTagQuery request, CancellationToken cancellationToken)
        {
            var topicsQuery = _dbContext.Topics
                .Include(topic => topic.Tags)
                .Where(topic => topic.Tags.Any(tag => tag.Name == request.TagName))
                .ProjectTo<TopicVm>(_mapper.ConfigurationProvider)
                .Sort(request.Sort);

            topicsQuery = topicsQuery.Paginate(request.Page, request.PageSize);

            int pageCount = await _dbContext.Topics
                .Include(topic => topic.Tags)
                .Where(topic => topic.Tags.Any(tag => tag.Name == request.TagName))
                .GetPageCount(request.PageSize);
            return new TopicListVm { Topics = await topicsQuery.ToListAsync(cancellationToken), PageCount = pageCount };
        }
    }
}
