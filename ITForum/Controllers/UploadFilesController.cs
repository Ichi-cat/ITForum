using ITForum.Application.Interfaces;
using ITForum.Controllers;
using ITForum.Domain.Errors.Generals;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>
        /// Upload file to topic
        /// </summary>
        /// <remarks>
        /// Send binary file in multipart/form-data
        /// 
        /// </remarks>
        /// <param name="file">IFormFile</param>
        /// <returns>Returns Success</returns>
        [SwaggerResponse(204)]
        [SwaggerResponse(400, type: typeof(GeneralExceptionVm))]
        [SwaggerResponse(401)]
        [SwaggerResponse(404, type: typeof(GeneralExceptionVm))]
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
