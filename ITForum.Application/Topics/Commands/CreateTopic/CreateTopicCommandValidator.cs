using FluentValidation;

namespace ITForum.Application.Topics.Commands.CreateTopic
{
    public class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
    {
        public CreateTopicCommandValidator()
        {
            RuleFor(createTopicCommand => createTopicCommand.Name)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(createTopicCommand => createTopicCommand.Content)
                .NotEmpty()
                .MaximumLength(1000);
            RuleFor(createTopicCommand => createTopicCommand.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
