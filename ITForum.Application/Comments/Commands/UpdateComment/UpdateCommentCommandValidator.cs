using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .MaximumLength(500);
            RuleFor(updateCommentCommand => updateCommentCommand.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
