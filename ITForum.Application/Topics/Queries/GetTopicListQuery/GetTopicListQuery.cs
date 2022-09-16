using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Topics.Queries.GetTopicListQuery
{
    public class GetTopicListQuery:IRequest<TopicListVM>
    {
        public Guid TopicId { get; set; }
        public string TopicName { get; set; }
    }
}
