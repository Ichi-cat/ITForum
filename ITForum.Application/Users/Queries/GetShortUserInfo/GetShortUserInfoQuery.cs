using MediatR;

namespace ITForum.Application.Users.Queries.GetShortUserInfo
{
    public class GetShortUserInfoQuery : IRequest<ShortUserInfoVm>
    {
        public Guid UserId { get; set; }
    }
}
