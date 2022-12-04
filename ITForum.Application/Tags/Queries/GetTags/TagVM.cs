using AutoMapper;
using ITForum.Application.Interfaces;
using ITForum.Domain.TopicItems;

namespace ITForum.Application.Tags.Queries.GetTags
{
    public class TagVM : IMap
    {
        public string Name { get; set; }
       
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Tag, TagVM>();
        }
    }
}
