using AutoMapper;
using ITForum.Application.Common.Mappings;
using ITForum.Domain.TopicItems;

namespace ITForum.Application.Topics.Services.LikesAndDislikes.Get
{
    public class MarkVM : IMap<Mark>
    {
        public Guid Id { get; set; }
        public Guid TopicId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Mark, MarkVM>();
        }
    }
}
