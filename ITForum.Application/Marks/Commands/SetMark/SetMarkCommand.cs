using ITForum.Domain.Enums;
using MediatR;

namespace ITForum.Application.Marks.Commands.SetMark
{
    public class SetMarkCommand : IRequest
    {
        public Guid UserId { get; set; }
        public MarkType IsLiked { get; set; }
        public Guid TopicId { get; set; }
    }
}
