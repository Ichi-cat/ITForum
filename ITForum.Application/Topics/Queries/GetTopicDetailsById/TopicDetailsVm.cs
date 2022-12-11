using AutoMapper;
using ITForum.Application.Interfaces;
using ITForum.Domain.TopicItems;

namespace ITForum.Application.Topics.Queries.GetTopicDetailsById
{
    //It's a class describing object to return for user
    public class TopicDetailsVm : IMap
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public List<Attachment>? Attachments { get; set; } // TODO: Запилить вывод через attachmentVm
        public List<string>? Tags { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public bool? IsLiked { get; set; } = null;
        public bool? IsDisliked { get; set; } = null;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Topic, TopicDetailsVm>();
        }
    }
}
