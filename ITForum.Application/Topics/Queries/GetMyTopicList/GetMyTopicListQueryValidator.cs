using FluentValidation;

namespace ITForum.Application.Topics.Queries.GetMyTopicList
{
    public class GetMyTopicListQueryValidator : AbstractValidator<GetMyTopicListQuery>
    {
        public GetMyTopicListQueryValidator()
        {
            RuleFor(getMyTopicListQuery => getMyTopicListQuery.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
