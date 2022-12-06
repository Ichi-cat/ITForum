using ITForum.Application.Tags.TagsViewModel;
using ITForum.Domain.Enums;
using MediatR;

namespace ITForum.Application.Tags.Queries.GetTags
{
    public class GetTagsListQuery : IRequest<TagListVM>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public TagSort Sort { get; set; }
    }
}
