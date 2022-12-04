using Microsoft.AspNetCore.Http;

namespace ITForum.Application.Interfaces
{
    public interface IBufferedFileUploadService
    {
        Task<List<string>> UploadFiles(IFormFile[] file);
        Task<string> UploadFile(IFormFile file, string? name = null);
    }
}
