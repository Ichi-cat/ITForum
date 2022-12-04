using ITForum.Application.Interfaces;
using ITForum.Domain.TopicItems;
using MediatR;

namespace ITForum.Application.Topics.Commands.UploadAttachments
{
    public class UploadAttachmentsCommandHandler : IRequestHandler<UploadAttachmentsCommand, List<Guid>>
    {
        private readonly IItForumDbContext _context;
        public UploadAttachmentsCommandHandler(IItForumDbContext context)
        {
            _context = context;
        }
        public async Task<List<Guid>> Handle(UploadAttachmentsCommand request, CancellationToken cancellationToken)
        {
            List<Guid> attachmentsId = new();
            foreach (var attachment in request.AttachmentsUrl)
            {
                var topicAttachment = new Attachment
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Url = attachment
                };
                await _context.Attachments.AddAsync(topicAttachment);
                attachmentsId.Add(topicAttachment.Id);
            }
            await _context.SaveChangesAsync();
            return attachmentsId;
        }
    }
}
