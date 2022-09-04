using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Domain.Topic
{
    internal class Comment
    {
        public int Id { get; set; }
        public int AuthorID { get; set; }
        public string Content { get; set; }
        public Topic? Topic { get; set; }
        public Comment? Comm { get; set; }
    }
}
