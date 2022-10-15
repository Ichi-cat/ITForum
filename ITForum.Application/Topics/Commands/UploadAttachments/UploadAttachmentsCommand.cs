using MediatR;
using Microsoft.AspNetCore.Http;

namespace ITForum.Application.Topics.Commands.UploadAttachments
{
    public class UploadAttachmentsCommand : IRequest<List<Guid>>
    {
        public Guid UserId { get; set; }
        public List<string> AttachmentsUrl { get; set; }
    }
}
