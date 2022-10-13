using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Marks.Queries.GetTopicLikesCountQuery
{
    public class GetTopicLikesCountQuery : IRequest<int>
    {
        public Guid UserId { get; set; }
        public Guid TopicId { get; set; }
    }
}
