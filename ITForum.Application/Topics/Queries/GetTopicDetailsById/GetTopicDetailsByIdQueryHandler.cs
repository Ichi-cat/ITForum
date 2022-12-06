using AutoMapper;
using ITForum.Application.Common.Exceptions;
using ITForum.Application.Interfaces;
using ITForum.Application.Tags.Queries.GetTopicTags;
using ITForum.Domain.Enums;
using ITForum.Domain.TopicItems;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Topics.Queries.GetTopicDetailsById
{
    public class GetTopicDetailsByIdQueryHandler : IRequestHandler<GetTopicDetailsByIdQuery, TopicDetailsVm>
    {
        private readonly IItForumDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public GetTopicDetailsByIdQueryHandler(IItForumDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<TopicDetailsVm> Handle(GetTopicDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var topic = await _context.Topics.Include(topic => topic.Attachments).FirstOrDefaultAsync(topic => topic.Id == request.Id);
            
            if (topic == null) throw new NotFoundException(nameof(Topic), request.Id);
            
            int CountLikes = await _context.Marks.CountAsync(mark => mark.TopicId == request.Id && mark.IsLiked == MarkType.LIKE);
            int CountDislikes = await _context.Marks.CountAsync(mark => mark.TopicId == request.Id && mark.IsLiked == MarkType.DISLIKE);

            var topicDetails = _mapper.Map<TopicDetailsVm>(topic);

            topicDetails.LikeCount = CountLikes;
            topicDetails.DislikeCount = CountDislikes;

            topicDetails.Tags = await _mediator.Send(new GetTopicTagsQuery { TopicId = request.Id });

            return topicDetails;
        }
    }
}
