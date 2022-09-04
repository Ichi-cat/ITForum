using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Domain.Topic
{
    internal class Comment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public Topic? Topic { get; set; }
        public Comment? Comm { get; set; }
    }
}
