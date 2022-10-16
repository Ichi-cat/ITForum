using AutoMapper;
using ITForum.Application.Interfaces;
using ITForum.Domain.TopicItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Topics.Queries.GetTopicListByTag
{
    public class TopicVM : IMap
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public List<Attachment> Attachments { get; set; }
        public List<Comment> Comment { get; set; }
        public DateTime Created { get; set; }
        public DateTime EditDate { get; set; }
        public List<Tag> Tags { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Topic, TopicVM>();
        }
    }
}
