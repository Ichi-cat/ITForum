using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Comments.Queries.GetComments
{
    public class GetCommentsQuery:IRequest<CommentListVM>
    {
        public Guid TopicId { get; set; }
        public int Count { get; set; }
    }
}
