using ITForum.Application.Interfaces;
using ITForum.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ITForum.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFilesController : BaseController
    {
        readonly IBufferedFileUploadService _bufferedFileUploadService;
        public UploadFilesController(IBufferedFileUploadService bufferedFileUploadService)
        {
            _bufferedFileUploadService = bufferedFileUploadService;
        }
        

        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile file)
        {
    
            try
            {
                if(await _bufferedFileUploadService.UploadFiles(file))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
