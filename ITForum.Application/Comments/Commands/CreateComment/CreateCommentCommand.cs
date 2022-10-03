using ITForum.Domain.TopicItems;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Comments.Commands.CreateComment
{
    public class CreateCommentCommand:IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public Guid TopicId { get; set; }= Guid.Empty;
        public Guid CommId { get; set; }=Guid.Empty;
    }
}
