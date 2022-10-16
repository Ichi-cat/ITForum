using ITForum.Application.Interfaces;
using ITForum.Application.Topics.Queries.GetTopicList;
using ITForum.Domain.TopicItems;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Tags.Commands.CreateTag
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Guid>
    {
        private readonly IItForumDbContext _context;
        public CreateTagCommandHandler(IItForumDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = new Tag
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return tag.Id;
        }
    }
}
