using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITForum.Application.Common.Extensions;
using ITForum.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Tags.Queries.GetTags
{
    public class GetTagsListQueryHandler : IRequestHandler<GetTagsListQuery, TagListVM>
    {
        private readonly IItForumDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetTagsListQueryHandler(IItForumDbContext dbContext,
         IMapper mapper) =>
         (_dbContext, _mapper) = (dbContext, mapper);
        public async Task<TagListVM> Handle(GetTagsListQuery request, CancellationToken cancellationToken)
        {
            var tagsQuery = await _dbContext.Tags
                .Paginate(request.Page, request.PageSize)
                .ProjectTo<TagVM>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            switch (request.Sort)
            {
                case Domain.Enums.TagSort.ASC:
                    break;
                case Domain.Enums.TagSort.DESC:
                    tagsQuery.Reverse();
                    break;
                case Domain.Enums.TagSort.PopularityASC:
                    //tagsQuery=tagsQuery.OrderByDescending(
                    /*topicQuery = topicQuery.OrderByDescending(topic => topic.Marks.Where(mark => mark.IsLiked == MarkType.LIKE).Count())
                            .ThenByDescending(topic => topic.Created);*/
                    break;
                default:
                    break;
            }

            int pageCount = await _dbContext.Tags
                .GetPageCount(request.PageSize);
            return new TagListVM { Tags = tagsQuery, PageCount = pageCount };
        }
    }
}
