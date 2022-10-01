using FluentValidation;

namespace ITForum.Application.Comments.Commands.DeleteComment
{
    internal class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentCommandValidator()
        {
            RuleFor(deleteCommentCommand => deleteCommentCommand.Id)
                .NotEqual(Guid.Empty);
            RuleFor(deleteCommentCommand => deleteCommentCommand.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
