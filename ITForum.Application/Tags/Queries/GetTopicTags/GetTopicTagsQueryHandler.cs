using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITForum.Application.Interfaces;
using ITForum.Application.Tags.TagsViewModel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Tags.Queries.GetTopicTags
{
    internal class GetTopicTagsQueryHandler : IRequestHandler<GetTopicTagsQuery, List<string>>
    {
        private readonly IItForumDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetTopicTagsQueryHandler(IItForumDbContext _dbcontext, IMapper mapper)
        {
            _dbContext = _dbcontext;
            _mapper = mapper;
        }

        public async Task<List<string>> Handle(GetTopicTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = await _dbContext.Tags
                .Where(t => t.Topics.Any(topic => topic.Id == request.TopicId))
                .Select(t => t.Name)
                .ToListAsync();

            return tags;
        }
    }
}
