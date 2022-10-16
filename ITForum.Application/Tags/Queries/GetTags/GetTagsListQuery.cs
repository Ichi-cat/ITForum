using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Tags.Queries.GetTags
{
    public class GetTagsListQuery : IRequest<TagListVM>
    {
        public string Name { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
