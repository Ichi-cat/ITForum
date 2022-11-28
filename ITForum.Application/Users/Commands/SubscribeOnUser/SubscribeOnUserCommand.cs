using MediatR;

namespace ITForum.Application.Users.Commands.SubscribeOnUser
{
    public class SubscribeOnUserCommand:IRequest
    {
        public Guid UserId { get; set; }
        public Guid SubscribeUserId { get; set; }
    }
}
