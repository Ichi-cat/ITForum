using AutoMapper;
using ITForum.Application.Interfaces;
using ITForum.Application.Users.Commands.UpdateUserInfo;

namespace ITForum.Api.Models
{
    public class UpdateUserInfoModel:IMap
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Description { get; set; }
        public string? Avatar { get; set; }
        public string? Location { get; set; }
        public string? Study { get; set; }
        public string? Work { get; set; }
        public DateTime? BirthDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUserInfoModel, UpdateUserInfoCommand>();
        }
    }
}
