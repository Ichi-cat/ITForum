using ITForum.Application.Interfaces;
using ITForum.Domain.TopicItems;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Tags.Commands.CreateTag
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, List<Tag>>
    {
        private readonly IItForumDbContext _context;
        public CreateTagCommandHandler(IItForumDbContext context)
        {
            _context = context;
        }
        public async Task<List<Tag>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tags = new List<Tag>();
            foreach (var tagName in request.TagNames)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    _context.Tags.Add(tag);
                }
                tags.Add(tag);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return tags;
        }
    }
}
