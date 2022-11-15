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
        //TODO: dont work (shows empty collection)
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

            int pageCount = await _dbContext.Tags
                .GetPageCount(request.PageSize);

            return new TagListVM { Tags = tagsQuery, PageCount = pageCount };
        }
    }
}
