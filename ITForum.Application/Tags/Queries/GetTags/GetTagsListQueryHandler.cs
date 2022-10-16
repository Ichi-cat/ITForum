using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITForum.Application.Comments.Queries.GetComments;
using ITForum.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

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
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<TagVM>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            
            int pageCount = tagsQuery.Count / request.PageSize;
            if (tagsQuery.Count % request.PageSize != 0) pageCount++;

            return new TagListVM { Tags = tagsQuery, PageCount = pageCount };
        }
    }
}
