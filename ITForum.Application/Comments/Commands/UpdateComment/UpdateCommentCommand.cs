using MediatR;

namespace ITForum.Application.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommand:IRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
    }
}
