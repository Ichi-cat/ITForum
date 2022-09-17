using AutoMapper;
using ITForum.Application.Common.Exceptions;
using ITForum.Application.Interfaces;
using ITForum.Domain.TopicItems;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Topics.Queries.GetTopicDetailsByIdQuery
{
    public class GetTopicDetailsByIdQueryHandler : IRequestHandler<GetTopicDetailsByIdQuery, TopicDetailsVm>
    {
        private readonly IItForumDbContext _context;
        private readonly IMapper _mapper;
        public GetTopicDetailsByIdQueryHandler(IItForumDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TopicDetailsVm> Handle(GetTopicDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            Topic topic = await _context.Topics.FirstOrDefaultAsync(topic => topic.Id == request.Id);
            if (topic == null) throw new NotFoundException(nameof(Topic), request.Id);
            return _mapper.Map<TopicDetailsVm>(topic);
        }
    }
}
