using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITForum.Api.enums;
using ITForum.Application.Common.Extensions;
using ITForum.Domain.ItForumUser;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Users.Queries.GetUserList
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, UserListVm>
    {
        private readonly UserManager<ItForumUser> _userManager;
        private readonly IMapper _mapper;

        public GetUserListQueryHandler(UserManager<ItForumUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<UserListVm> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            List<ItForumUser> subscriptions;
            if (request.UserId == Guid.Empty)
            {
                subscriptions = new List<ItForumUser>();
            }
            else
            {
                subscriptions = await _userManager.Users.Where(u => u.Subscribers.Any(u => u.Id == request.UserId)).ToListAsync();
            }
            var query = _userManager.Users
                .Include(u => u.Subscriptions)
                .ProjectTo<UserItemVm>(_mapper.ConfigurationProvider);

            switch (request.Sort)
            {
                case UsersSort.ByNameAsc:
                    query = query.OrderBy(u => u.FirstName);
                    break;
                case UsersSort.ByNameDesc:
                    query = query.OrderByDescending(u => u.FirstName);
                    break;
                default:
                    break;
            }
            query = query.Paginate(request.Page, request.PageSize);

            int pagesCount = await _userManager.Users.GetPageCount(request.PageSize);
            var users = await query.ToListAsync();
            users.ForEach(u => u.IsSubscribed = subscriptions.Any(s => s.Id == u.Id));

            return new UserListVm { Users = users, PagesCount = pagesCount };
        }
    }
}
