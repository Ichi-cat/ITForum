using MediatR;

namespace ITForum.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest
    {
        public Guid UserId { get; set; }
    }
}
