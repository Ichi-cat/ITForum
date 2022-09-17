using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITForum.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Topics.Queries.GetTopicListQuery
{
    internal class GetTopicListQueryHandler : IRequestHandler<GetTopicListQuery, TopicListVM>
    {
        private readonly IItForumDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetTopicListQueryHandler(IItForumDbContext dbContext,
            IMapper mapper) =>
            (_dbContext, _mapper) = (dbContext, mapper);
        public async Task<TopicListVM> Handle(GetTopicListQuery request, CancellationToken cancellationToken)
        {
            var topicQuery = await _dbContext.Topics
                .Skip(request.Start).Take(request.Tail)
                .ProjectTo<TopicVM>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return new TopicListVM { Topics = topicQuery };
        }
    }
}
