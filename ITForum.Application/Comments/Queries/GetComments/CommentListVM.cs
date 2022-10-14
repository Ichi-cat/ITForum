using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Comments.Queries.GetComments
{
    internal class CommentListVM
    {
        public IList<CommentVM> Comments { get; set; }
    }
}
