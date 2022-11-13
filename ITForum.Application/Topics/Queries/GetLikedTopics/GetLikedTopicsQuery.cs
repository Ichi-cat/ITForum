using MediatR;

namespace ITForum.Application.Topics.Queries.GetLikedTopics
{
    public class GetLikedTopicsQuery : IRequest<LikedTopicsListVm>
    {
        public Guid UserId { get; set; }
    }
}
