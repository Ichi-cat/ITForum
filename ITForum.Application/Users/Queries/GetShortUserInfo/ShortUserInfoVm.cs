using AutoMapper;
using ITForum.Application.Interfaces;
using ITForum.Domain.ItForumUser;

namespace ITForum.Application.Users.Queries.GetShortUserInfo
{
    public class ShortUserInfoVm:IMap
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public ICollection<string> Roles { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ItForumUser, ShortUserInfoVm>()
                .ForMember(vm => vm.UserName, opt => opt.MapFrom(user => user.UserName))
                .ForMember(vm => vm.Email, opt => opt.MapFrom(user => user.Email))
                .ForMember(vm => vm.Avatar, opt => opt.MapFrom(user => user.Avatar));
        }
    }
}
