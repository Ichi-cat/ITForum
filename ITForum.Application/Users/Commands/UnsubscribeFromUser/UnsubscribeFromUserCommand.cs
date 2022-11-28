using MediatR;

namespace ITForum.Application.Users.Commands.UnsubscribeFromUser
{
    public class UnsubscribeFromUserCommand : IRequest
    {
        public Guid UserId { get; set; }
        public Guid UnsubscribeUserId { get; set; }
    }
}
