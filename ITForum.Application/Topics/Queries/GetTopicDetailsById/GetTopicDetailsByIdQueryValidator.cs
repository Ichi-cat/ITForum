using FluentValidation;

namespace ITForum.Application.Topics.Queries.GetTopicDetailsById
{
    public class GetTopicDetailsByIdQueryValidator : AbstractValidator<GetTopicDetailsByIdQuery>
    {
        public GetTopicDetailsByIdQueryValidator()
        {
            RuleFor(getTopicDetailsByIdQuery => getTopicDetailsByIdQuery.Id)
                .NotEqual(Guid.Empty);
            RuleFor(getTopicDetailsByIdQuery => getTopicDetailsByIdQuery.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
