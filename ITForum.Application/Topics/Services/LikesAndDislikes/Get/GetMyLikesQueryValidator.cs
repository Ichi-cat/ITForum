using FluentValidation;

namespace ITForum.Application.Topics.Services.LikesAndDislikes.Get
{
    internal class GetMyLikesQueryValidator : AbstractValidator<GetMyLikesQuery>
    {
        public GetMyLikesQueryValidator()
        {
            RuleFor(getMyLikesQuery => getMyLikesQuery.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
