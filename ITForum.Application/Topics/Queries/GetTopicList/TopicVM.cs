using AutoMapper;
using ITForum.Application.Interfaces;
using ITForum.Domain.TopicItems;

namespace ITForum.Application.Topics.Queries.GetTopicList
{
    internal class TopicVM : IMap
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public List<Attachment> Attachments { get; set; }
        public List<Comment> Comment { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Topic, TopicVM>();
        }
    }
}