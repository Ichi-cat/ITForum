using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITForum.Application.Common.Extensions;
using ITForum.Application.Interfaces;
using ITForum.Application.Topics.TopicViewModels;
using ITForum.Domain.Enums;
using ITForum.Domain.ItForumUser;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Topics.Queries.GetTopicsBySubscriptions
{
    public class GetTopicsBySubscriptionsQueryHandler : IRequestHandler<GetTopicsBySubscriptionsQuery, TopicListVm>
    {
        private readonly IItForumDbContext _context;
        private readonly UserManager<ItForumUser> _userManager;
        private readonly IMapper _mapper;

        public GetTopicsBySubscriptionsQueryHandler(IItForumDbContext context, UserManager<ItForumUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<TopicListVm> Handle(GetTopicsBySubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var userIds = await _userManager.Users.Where(u => u.Subscribers.Any(u => u.Id == request.UserId)).Select(u => u.Id).ToListAsync();
            var topicsQuery = _context.Topics
                .Where(topic => userIds.Any(id => id == topic.UserId))
                .ProjectTo<TopicVm>(_mapper.ConfigurationProvider);
            switch (request.Sort)
            {
                case TypeOfSort.ByDateASC:
                    topicsQuery = topicsQuery.OrderBy(topic => topic.Created);
                    break;
                case TypeOfSort.ByDateDESC:
                    topicsQuery = topicsQuery.OrderByDescending(topic => topic.Created);
                    break;
                case TypeOfSort.ByRatingASC:
                    topicsQuery = topicsQuery.OrderByDescending(topic => topic.Marks.Where(mark => mark.IsLiked == MarkType.LIKE).Count())
                        .ThenByDescending(topic => topic.Created);
                    break;
            }
            topicsQuery = topicsQuery.Paginate(request.Page, request.PageSize);
            var pageCount = await _context.Topics
                .Where(topic => userIds.Any(id => id == topic.UserId)).GetPageCount(request.PageSize);

            return new TopicListVm { Topics = await topicsQuery.ToListAsync(), PageCount=pageCount };
        }
    }
}
