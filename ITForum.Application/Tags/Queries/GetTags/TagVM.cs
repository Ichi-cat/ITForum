using AutoMapper;
using ITForum.Application.Comments.Queries.GetComments;
using ITForum.Domain.TopicItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Tags.Queries.GetTags
{
    public class TagVM
    {
        public string Name { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Tag, TagVM>();
        }
    }
}
