using AutoMapper;
using ITForum.Application.Common.Mappings;
using ITForum.Application.Topics.Queries.GetTopicListQuery;
using ITForum.Domain.TopicItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ITForum.Domain.TopicItems.Mark;

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
