using FluentValidation;

namespace ITForum.Application.Topics.Commands.UpdateTopic
{
    public class UpdateTopicCommandValidator : AbstractValidator<UpdateTopicCommand>
    {
        public UpdateTopicCommandValidator()
        {
            RuleFor(updateTopicCommand => updateTopicCommand.Id)
                .NotEqual(Guid.Empty);
            RuleFor(updateTopicCommand => updateTopicCommand.UserId)
                .NotEqual(Guid.Empty);
            RuleFor(updateTopicCommand => updateTopicCommand.Name)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(updateTopicCommand => updateTopicCommand.Content)
                .NotEmpty()
                .MaximumLength(1000);
        }
    }
}
