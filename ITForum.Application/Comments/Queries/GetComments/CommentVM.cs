using AutoMapper;
using ITForum.Application.Interfaces;
using ITForum.Domain.TopicItems;

namespace ITForum.Application.Comments.Queries.GetComments
{
    internal class CommentVM : IMap
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public Guid? TopicId { get; set; }
        public Guid? CommId { get; set; }
        public List<Mark> Marks { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Comment, CommentVM>();
        }
    }
}
