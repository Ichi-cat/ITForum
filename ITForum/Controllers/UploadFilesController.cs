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

        /// <summary>
        /// Upload file to topic
        /// </summary>
        /// <remarks>
        /// Send binary file in multipart/form-data
        /// 
        /// </remarks>
        /// <param name="file">IFormFile</param>
        /// <response code="200">Success</response>
        /// todo: 400 code(bad request)
        /// todo: 500? internal error
        /// <response code="401">User is unauthorized</response>
        /// <returns>Returns Success</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
