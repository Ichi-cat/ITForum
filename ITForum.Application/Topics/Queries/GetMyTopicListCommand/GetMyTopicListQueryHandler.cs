using ITForum.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Topics.Queries.GetMyTopicListCommand
{
    public class GetMyTopicListQueryHandler : IRequestHandler<GetMyTopicListQuery, TopicListVm>
    {
        private readonly IItForumDbContext _context;
        public GetMyTopicListQueryHandler(IItForumDbContext context)
        {
            _context = context;
        }
        public async Task<TopicListVm> Handle(GetMyTopicListQuery request, CancellationToken cancellationToken)
        {
            var topics = await _context.Topics.Where(topic => topic.UserId == request.UserId)
                .Select(topic => new TopicItemVm { Id=topic.Id, Name = topic.Name })
                .ToListAsync();

            return new TopicListVm { Topics = topics };
        }
    }
}
