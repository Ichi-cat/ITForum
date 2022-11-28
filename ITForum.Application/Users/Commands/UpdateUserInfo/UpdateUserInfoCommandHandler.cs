using ITForum.Domain.ItForumUser;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ITForum.Application.Users.Commands.UpdateUserInfo
{
    public class UpdateUserInfoCommandHandler : IRequestHandler<UpdateUserInfoCommand>
    {
        private readonly UserManager<ItForumUser> _userManager;

        public UpdateUserInfoCommandHandler(UserManager<ItForumUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Unit> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
        {
            ItForumUser user = await _userManager.FindByIdAsync(request.UserId.ToString());

            user.FirstName = request.FirstName ?? user.FirstName;
            user.LastName = request.LastName ?? user.LastName;
            user.Description = request.Description ?? user.Description;
            user.Avatar = request.Avatar ?? user.Avatar;
            user.Location = request.Location ?? user.Location;
            user.BirthDate = request.BirthDate ?? user.BirthDate;
            user.Study = request.Study ?? user.Study;
            user.Work = request.Work ?? user.Work;
            await _userManager.UpdateAsync(user);
            return Unit.Value;
        }
    }
}
