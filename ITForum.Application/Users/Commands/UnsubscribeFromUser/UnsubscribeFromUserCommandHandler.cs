using ITForum.Domain.ItForumUser;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Users.Commands.UnsubscribeFromUser
{
    public class UnsubscribeFromUserCommandHandler : IRequestHandler<UnsubscribeFromUserCommand>
    {
        private readonly UserManager<ItForumUser> _userManager;

        public UnsubscribeFromUserCommandHandler(UserManager<ItForumUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Unit> Handle(UnsubscribeFromUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(u => u.Subscriptions).FirstOrDefaultAsync(u => u.Id == request.UserId);
            var userSub = await _userManager.FindByIdAsync(request.UnsubscribeUserId.ToString());
            if (userSub == null)
            {
                throw new Exception("User not found");
            }
            user?.Subscriptions.Remove(userSub);
            if (user != null) await _userManager.UpdateAsync(user);
            return Unit.Value;
        }
    }
}
