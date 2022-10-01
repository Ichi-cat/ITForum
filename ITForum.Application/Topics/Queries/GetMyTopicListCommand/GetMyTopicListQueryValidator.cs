using FluentValidation;

namespace ITForum.Application.Topics.Queries.GetMyTopicListCommand
{
    internal class GetMyTopicListQueryValidator : AbstractValidator<GetMyTopicListQuery>
    {
        public GetMyTopicListQueryValidator()
        {
            RuleFor(getMyTopicListQuery => getMyTopicListQuery.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
