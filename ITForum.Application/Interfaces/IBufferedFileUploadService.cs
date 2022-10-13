using Microsoft.AspNetCore.Http;

namespace ITForum.Application.Interfaces
{
    public interface IBufferedFileUploadService
    {
        Task<bool> UploadFiles(IFormFile file);
    }
}
