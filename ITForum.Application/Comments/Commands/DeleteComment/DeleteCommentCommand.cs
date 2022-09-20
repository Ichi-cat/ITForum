using MediatR;

namespace ITForum.Application.Comments.Commands.DeleteComment
{
    internal class DeleteCommentCommand:IRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
