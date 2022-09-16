using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Topics.Queries.GetTopicListQuery
{
    internal class TopicListVM
    {
        public IList<TopicVM> Topics { get; set; }
    }
}
