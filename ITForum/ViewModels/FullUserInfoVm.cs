using AutoMapper;
using ITForum.Application.Interfaces;
using ITForum.Domain.ItForumUser;

namespace ITForum.Api.ViewModels
{
    public class FullUserInfoVm : IMap
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public string Description { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ItForumUser, FullUserInfoVm>()
                .ForMember(vm => vm.UserName, opt => opt.MapFrom(user => user.UserName))
                .ForMember(vm => vm.Email, opt => opt.MapFrom(user => user.Email))
                .ForMember(vm => vm.Avatar, opt => opt.MapFrom(user => user.Avatar))
                .ForMember(vm => vm.FirstName, opt => opt.MapFrom(user => user.FirstName))
                .ForMember(vm => vm.LastName, opt => opt.MapFrom(user => user.LastName))
                .ForMember(vm => vm.Description, opt => opt.MapFrom(user => user.Description));
        }
    }
}
