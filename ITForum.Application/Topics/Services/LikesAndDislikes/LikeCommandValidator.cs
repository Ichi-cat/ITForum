﻿using FluentValidation;

namespace ITForum.Application.Topics.Services.LikesAndDislikes
{
    internal class LikeCommandValidator : AbstractValidator<LikeCommand>
    {
        public LikeCommandValidator()
        {
            RuleFor(likeCommand => likeCommand.TopicId)
                .NotEqual(Guid.Empty);
            RuleFor(likeCommand => likeCommand.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}