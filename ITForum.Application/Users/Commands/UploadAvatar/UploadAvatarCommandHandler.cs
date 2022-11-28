using ITForum.Application.Common.Exceptions;
using ITForum.Application.Interfaces;
using ITForum.Domain.ItForumUser;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SixLabors.ImageSharp;

namespace ITForum.Application.Users.Commands.UploadAvatar
{
    public class UploadAvatarCommandHandler : IRequestHandler<UploadAvatarCommand, string>
    {
        private readonly UserManager<ItForumUser> _userManager;
        private readonly IBufferedFileUploadService _uploadFile;

        public UploadAvatarCommandHandler(UserManager<ItForumUser> userManager, IBufferedFileUploadService uploadFile)
        {
            _userManager = userManager;
            _uploadFile = uploadFile;
        }
        public async Task<string> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
        {
            if (request.Avatar.ContentType != "image/jpeg"
                && request.Avatar.ContentType != "image/png")
            {
                throw new UploadFileException("File type is not supported");
            }
            if ((request.Avatar.Length / 1024 / 1024) > 5) throw new UploadFileException("Max size is 5MB");
            using (var stream = request.Avatar.OpenReadStream())
            {
                using (var image = Image.Load(stream))
                {
                    if (image.Height != image.Width) throw new UploadFileException("Height must equal width");
                }
            }
            var extension = Path.GetExtension(request.Avatar.FileName);
            if (extension == null || extension == "")
            {
                extension = ".jpg";
            }
            var path = (await _uploadFile.UploadFile(request.Avatar, Guid.NewGuid().ToString() + extension));
            path = request.Path + path;
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            user.Avatar = path;
            await _userManager.UpdateAsync(user);
            return path;
        }
    }
}
