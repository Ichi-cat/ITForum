using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITForum.Application.Interfaces;
using ITForum.Application.Tags.Queries.GetTags;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Topics.Queries.GetTopicListByTag
{
    public class GetTopicListByTagQueryHandler : IRequestHandler<GetTopicListByTagQuery, TopicListVM>
    {
        private readonly IItForumDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetTopicListByTagQueryHandler(IItForumDbContext dbContext,
            IMapper mapper) =>
            (_dbContext, _mapper) = (dbContext, mapper);
        public async Task<TopicListVM> Handle(GetTopicListByTagQuery request, CancellationToken cancellationToken)
        {
            List<TopicVM> topicQuery = await _dbContext.Topics
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<TopicVM>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            var responseList = from topic in topicQuery
                               from tag in topic.Tags
                               where tag.Name == request.TagName
                               select topic;
            int pageCount = topicQuery.Count / request.PageSize;
            if (topicQuery.Count % request.PageSize != 0) pageCount++;

            return new TopicListVM { Topics = topicQuery, PageCount = pageCount };
        }
    }
}
