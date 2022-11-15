using AutoMapper;
using ITForum.Application.Interfaces;
using ITForum.Domain.TopicItems;

namespace ITForum.Application.Topics.TopicViewModels
{
    public class TopicVm : IMap
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortContent { get; set; }
        public DateTime Created { get; set; }
        public List<Mark> Marks { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Topic, TopicVm>()
                .ForMember(d => d.ShortContent, opt => opt.MapFrom(s => s.Content.Substring(0, 200)));
        }
    }
}
