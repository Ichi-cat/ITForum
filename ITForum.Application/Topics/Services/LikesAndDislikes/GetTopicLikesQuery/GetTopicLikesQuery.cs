using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Topics.Services.LikesAndDislikes.GetTopicLikesQuery
{
    public class GetTopicLikesQuery : IRequest<int>
    {
        public Guid UserId { get; set; }
        public Guid TopicId { get; set; }
    }
}
