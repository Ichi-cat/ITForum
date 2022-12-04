using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITForum.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Comments.Queries.GetComments
{
    internal class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, CommentListVM>
    {
        private readonly IItForumDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCommentsQueryHandler(IItForumDbContext dbContext,
            IMapper mapper) =>
            (_dbContext, _mapper) = (dbContext, mapper);
        public async Task<CommentListVM> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
        {
            var commentQuery = await _dbContext.Comments
                .Take(request.Count)
                .ProjectTo<CommentVM>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return new CommentListVM { Comments = commentQuery };
        }
    }
}
