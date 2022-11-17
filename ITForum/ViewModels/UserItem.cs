﻿using AutoMapper;
using ITForum.Application.Interfaces;
using ITForum.Domain.ItForumUser;

namespace ITForum.Api.ViewModels
{
    public class UserItem : IMap
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string FullName { get; set; } = null!;
        public string? Avatar { get; set; }

        public bool IsSubscribed { get; set; } = false;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ItForumUser, UserItem>()
                .ForMember(i => i.FullName, map => map.MapFrom(u => u.FullName));
        }
    }
}
