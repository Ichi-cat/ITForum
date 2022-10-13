using ITForum.Application.Interfaces;
using ITForum.Domain.TopicItems;
using MediatR;

namespace ITForum.Application.Comments.Commands.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Guid>
    {
        private readonly IItForumDbContext _context;
        public CreateCommentCommandHandler(IItForumDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            Comment comment = new Comment
            {
                UserId = request.UserId,
                Content = request.Content,
                TopicId=request.TopicId,
                Id = Guid.NewGuid(),
            };
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment.Id;
        }
    }
}
