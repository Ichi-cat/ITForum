using System;
using FluentValidation;

namespace ITForum.Application.Comments.Commands.CreateComment
{
    internal class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(createCommentCommand => createCommentCommand.Content)
                .NotEmpty()
                .MaximumLength(500);
            RuleFor(createCommentCommand => createCommentCommand.UserId)
                .NotEqual(Guid.Empty);
            RuleFor(createCommentCommand => createCommentCommand.Topic)
                .NotNull();
            RuleFor(createCommentCommand => createCommentCommand.Comm)
                .NotNull();
        }
    }
}
