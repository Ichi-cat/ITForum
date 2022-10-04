using FluentValidation;

namespace ITForum.Application.Topics.Commands.DeleteTopic
{
    public class DeleteTopicCommandValidator : AbstractValidator<DeleteTopicCommand>
    {
        public DeleteTopicCommandValidator()
        {
            RuleFor(deleteTopicCommand => deleteTopicCommand.Id)
                .NotEqual(Guid.Empty);
            RuleFor(deleteTopicCommand => deleteTopicCommand.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
