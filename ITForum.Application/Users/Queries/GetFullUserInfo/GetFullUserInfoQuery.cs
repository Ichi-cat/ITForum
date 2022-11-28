using MediatR;

namespace ITForum.Application.Users.Queries.GetFullUserInfo
{
    public class GetFullUserInfoQuery:IRequest<FullUserInfoVm>
    {
        public Guid UserId { get; set; }
    }
}
