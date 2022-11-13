using AutoMapper;
using ITForum.Application.Interfaces;
using ITForum.Application.Topics.Queries.GetTopicDetailsById;
using ITForum.Domain.TopicItems;

namespace ITForum.Application.Topics.Queries.GetLikedTopics
{
    public class LikedTopicVm : IMap
    {
        public Guid TopicId { get; set; }
        public string Name { get; set; }
        public string ShortContent { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Topic, LikedTopicVm>()
                .ForMember(d => d.ShortContent, opt => opt.MapFrom(s => s.Content.Substring(0, 200)))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.TopicId, opt => opt.MapFrom(s => s.Id));
        }
    }
}
