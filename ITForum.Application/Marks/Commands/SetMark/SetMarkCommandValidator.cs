using FluentValidation;

namespace ITForum.Application.Marks.Commands.SetMark
{
    public class SetMarkCommandValidator : AbstractValidator<SetMarkCommand>
    {
        public SetMarkCommandValidator()
        {
            RuleFor(likeCommand => likeCommand.TopicId)
                .NotEqual(Guid.Empty);
            RuleFor(likeCommand => likeCommand.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
