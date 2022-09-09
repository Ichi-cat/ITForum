using ITForum.Application.Common.Exceptions;
using ITForum.Application.Interfaces;
using ITForum.Domain.TopicItems;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Topics.Queries.GetTopicDetailsByIdQuery
{
    public class GetTopicDetailsByIdQueryHandler : IRequestHandler<GetTopicDetailsByIdQuery, Topic>
    {
        private readonly IItForumDbContext _context;
        public GetTopicDetailsByIdQueryHandler(IItForumDbContext context)
        {
            _context = context;
        }
        public async Task<Topic> Handle(GetTopicDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            Topic topic = await _context.Topics.FirstOrDefaultAsync(topic => topic.Id == request.Id);
            if (topic == null) throw new NotFoundException();
            return topic;
        }
    }
}
