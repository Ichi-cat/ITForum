using AutoMapper;
using ITForum.Application.Interfaces;
using ITForum.Domain.ItForumUser;

namespace ITForum.Application.Users.Queries.GetFullUserInfo
{
    public class FullUserInfoVm : IMap
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Avatar { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? Location { get; set; } = null;
        public string? BirthLocation { get; set; } = null;
        public DateTime? BirthDate { get; set; } = null;
        public string? Study { get; set; } = null;
        public string? Work { get; set; } = null;
        public string? TimeZone { get; set; } = null;

        public string FullName { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ItForumUser, FullUserInfoVm>()
                .ForMember(vm => vm.UserName, opt => opt.MapFrom(user => user.UserName))
                .ForMember(vm => vm.Email, opt => opt.MapFrom(user => user.Email))
                .ForMember(vm => vm.Avatar, opt => opt.MapFrom(user => user.Avatar))
                .ForMember(vm => vm.FirstName, opt => opt.MapFrom(user => user.FirstName))
                .ForMember(vm => vm.LastName, opt => opt.MapFrom(user => user.LastName))
                .ForMember(vm => vm.Description, opt => opt.MapFrom(user => user.Description))
                .ForMember(vm => vm.Location, opt => opt.MapFrom(user => user.Location))
                .ForMember(vm => vm.BirthLocation, opt => opt.MapFrom(user => user.BirthLocation))
                .ForMember(vm => vm.BirthDate, opt => opt.MapFrom(user => user.BirthDate))
                .ForMember(vm => vm.Study, opt => opt.MapFrom(user => user.Study))
                .ForMember(vm => vm.Work, opt => opt.MapFrom(user => user.Work))
                .ForMember(vm => vm.TimeZone, opt => opt.MapFrom(user => user.TimeZone))
                .ForMember(vm => vm.FullName, opt => opt.MapFrom(user => user.FullName));
        }
    }
}