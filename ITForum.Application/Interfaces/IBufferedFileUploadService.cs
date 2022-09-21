using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Interfaces
{
    public interface IBufferedFileUploadService
    {
        Task<bool> UploadFiles(IFormFile file);
    }
}
