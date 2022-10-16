using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Topics.Queries.GetTopicListByTag
{
    public class TopicListVM
    {
        public IList<TopicVM> Topics { get; set; }
        public int PageCount { get; set; }
    }
}
