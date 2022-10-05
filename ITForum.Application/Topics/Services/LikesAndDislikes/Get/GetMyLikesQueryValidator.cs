using FluentValidation;

namespace ITForum.Application.Topics.Services.LikesAndDislikes.Get
{
    public class GetMyLikesQueryValidator : AbstractValidator<GetMyLikesQuery>
    {
        public GetMyLikesQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
