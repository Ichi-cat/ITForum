using ITForum.Domain.ItForumUser;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ITForum.Application.Users.Commands.DeleteUser
{
    internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly UserManager<ItForumUser> _userManager;

        public DeleteUserCommandHandler(UserManager<ItForumUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            await _userManager.DeleteAsync(user);
            
            return Unit.Value;
        }
    }
}
