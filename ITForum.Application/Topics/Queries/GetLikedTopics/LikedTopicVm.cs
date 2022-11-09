using AutoMapper;
using ITForum.Application.Interfaces;
using ITForum.Application.Topics.Queries.GetTopicDetailsById;
using ITForum.Domain.TopicItems;

namespace ITForum.Application.Topics.Queries.GetLikedTopics
{
    public class LikedTopicVm
    {
        public Guid TopicId { get; set; }
        public string Name { get; set; }
        public string ShortContent { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Topic, LikedTopicVm>();
        }
    }
}
