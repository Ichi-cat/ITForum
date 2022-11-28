using ITForum.Api.enums;
using MediatR;

namespace ITForum.Application.Users.Queries.GetUserList
{
    public class GetUserListQuery:IRequest<UserListVm>
    {
        public Guid UserId { get; set; }
        public UsersSort Sort { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
