using AutoMapper;
using ITForum.Application.Common.Exceptions;
using ITForum.Domain.ItForumUser;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ITForum.Application.Users.Queries.GetFullUserInfo
{
    public class GetFullUserInfoQueryHandler : IRequestHandler<GetFullUserInfoQuery, FullUserInfoVm>
    {
        private readonly UserManager<ItForumUser> _userManager;
        private readonly IMapper _mapper;

        public GetFullUserInfoQueryHandler(UserManager<ItForumUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<FullUserInfoVm> Handle(GetFullUserInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null) throw new UnauthorizeException();
            return _mapper.Map<FullUserInfoVm>(user);
        }
    }
}
