using MediatR;
using Microsoft.AspNetCore.Http;

namespace ITForum.Application.Users.Commands.UploadAvatar
{
    public class UploadAvatarCommand:IRequest<string>
    {
        public Guid UserId { get; set; }
        public IFormFile Avatar { get; set; } = null!;
        public string Path { get; set; } = null!;
    }
}
