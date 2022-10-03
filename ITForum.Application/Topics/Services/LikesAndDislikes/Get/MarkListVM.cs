using ITForum.Application.Topics.Queries.GetTopicListQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Topics.Services.LikesAndDislikes.Get
{
    internal class MarkListVM
    {
        public IList<MarkVM> Marks { get; set; }
    }
}
