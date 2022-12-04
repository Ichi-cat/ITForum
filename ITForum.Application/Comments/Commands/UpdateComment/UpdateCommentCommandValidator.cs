using FluentValidation;

namespace ITForum.Application.Comments.Commands.UpdateComment
{
    internal class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(updateCommentCommand => updateCommentCommand.Id)
                .NotEqual(Guid.Empty);
            RuleFor(updateCommentCommand => updateCommentCommand.Content)
                .NotEmpty()
                .NotNull()
                .MaximumLength(500);
            RuleFor(updateCommentCommand => updateCommentCommand.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
