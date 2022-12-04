using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Tags.Queries.GetTags
{
    public class TagListVM
    {
        public IList<TagVM> Tags { get; set; }
        public int PageCount { get; set; }
    }
}
