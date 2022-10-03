using ITForum.Domain.TopicItems;
using MediatR;
using static ITForum.Domain.TopicItems.Mark;

namespace ITForum.Application.Topics.Services.LikesAndDislikes
{
    public class LikeCommand : IRequest
    {
        public Guid UserId { get; set; }
        public MarkType IsLiked { get; set; }
        public Guid TopicId { get; set; }
    }
}
