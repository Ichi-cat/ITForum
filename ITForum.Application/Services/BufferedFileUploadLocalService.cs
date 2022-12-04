using ITForum.Application.Common.Exceptions;
using ITForum.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ITForum.Application.Services
{
    public class BufferedFileUploadLocalService : IBufferedFileUploadService
    {
        public async Task<List<string>> UploadFiles(IFormFile[] file)
        {
            string path = "";
            try
            {
                if (file.Length > 0)
                {
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    List<string> resultUrl = new();

                    foreach (var formFile in file)
                    {
                        if (formFile.Length > 0)
                        {
                            var filePath = Path.Combine(path, formFile.FileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(stream);
                            }
                            resultUrl.Add(formFile.FileName);
                        }
                    }
                    return resultUrl;
                }
                else
                {
                    throw new Exception(); // TODO: Create custom exception
                }
            }
            catch (Exception ex)
            {
                throw new UploadFileException("File uploading is failed", ex);
            }
        }
        public async Task<string> UploadFile(IFormFile file, string? name = null)
        {
            string path = "";
            try
            {
                if (file.Length > 0)
                {
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string resultUrl;

                    var filePath = Path.Combine(path, name ?? file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    resultUrl = name ?? file.FileName;
                    return resultUrl;
                }
                else
                {
                    throw new Exception(); // TODO: Create custom exception
                }
            }
            catch (Exception ex)
            {
                throw new UploadFileException("File uploading is failed", ex);
            }
        }
    }
}
