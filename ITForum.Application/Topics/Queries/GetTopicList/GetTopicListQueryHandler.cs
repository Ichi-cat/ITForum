using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITForum.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ITForum.Domain.Enums;

namespace ITForum.Application.Topics.Queries.GetTopicList
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
            var topicQuery = _dbContext.Topics
                .Include(topic => topic.Marks)
                .ProjectTo<TopicVM>(_mapper.ConfigurationProvider);
                
            switch (request.Sort)
            {
                case TypeOfSort.ByDateASC:
                    topicQuery = topicQuery.OrderBy(topic => topic.Created);
                    break;
                case TypeOfSort.ByDateDESC:
                    topicQuery = topicQuery.OrderByDescending(topic => topic.Created);
                    break;
                case TypeOfSort.ByRatingASC:
                    topicQuery = topicQuery.OrderBy(topic => topic.Marks.Where(mark => mark.IsLiked == MarkType.LIKE).Count());
                    break;
            }
            topicQuery = topicQuery.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
            return new TopicListVM { Topics = await topicQuery.ToListAsync(cancellationToken) };
        }
    }
}
