using FluentValidation;

namespace ITForum.Application.Topics.Queries.GetMyTopicList
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
