using AutoMapper;
using ITForum.Domain.ItForumUser;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ITForum.Application.Users.Queries.GetShortUserInfo
{
    public class GetShortUserInfoQueryHandler : IRequestHandler<GetShortUserInfoQuery, ShortUserInfoVm>
    {
        private readonly UserManager<ItForumUser> _userManager;
        private readonly IMapper _mapper;

        public GetShortUserInfoQueryHandler(UserManager<ItForumUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<ShortUserInfoVm> Handle(GetShortUserInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            var userInfo = _mapper.Map<ShortUserInfoVm>(user);
            userInfo.Roles = await _userManager.GetRolesAsync(user);
            return userInfo;
        }
    }
}
