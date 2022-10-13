using AutoMapper;
using ITForum.Application.Interfaces;
using ITForum.Domain.TopicItems;

namespace ITForum.Application.Topics.Queries.GetTopicDetailsById
{
    //It's a class describing object to return for user
    public class TopicDetailsVm : IMap
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public List<Attachment> Attachment { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Topic, TopicDetailsVm>();
        }
    }
}
