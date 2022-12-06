using FluentValidation;

namespace ITForum.Application.Topics.Queries.GetTopicListByUser
{
    public class GetTopicListByUserQueryValidator : AbstractValidator<GetTopicListByUserQuery>
    {
        public GetTopicListByUserQueryValidator()
        {
            RuleFor(getMyTopicListQuery => getMyTopicListQuery.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
