﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Topics.Queries.GetTopicListByTag
{
    public class GetTopicListByTagQuery : IRequest<TopicListVM>
    {
        public string TagName { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}