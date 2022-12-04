using ITForum.Domain.Enums;
using MediatR;
using System.Text;

namespace ITForum.Application.Tags.Queries.GetTags
{
    public class GetTagsListQuery : IRequest<TagListVM>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public TagSort Sort { get; set; }
    }
}
