using ITForum.Domain.TopicItems;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommand:IRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public Topic? Topic { get; set; }
        public Comment? Comm { get; set; }
    }
}
