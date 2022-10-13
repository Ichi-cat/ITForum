using MediatR;

namespace ITForum.Application.Comments.Commands.CreateComment
{
    public class CreateCommentCommand:IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public Guid TopicId { get; set; }
        public Guid CommId { get; set; }
    }
}
