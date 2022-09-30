using ITForum.Api.Models;
using ITForum.Application.Topics.Services.LikesAndDislikes;
using ITForum.Application.Topics.Services.LikesAndDislikes.Get;
using ITForum.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ITForum.Api.Controllers
{
    public class LikeController : BaseController
    {
        /// <summary>
        /// Like or dislike topic
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        /// 
        ///     Put
        ///     {
        ///         "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "isLiked": 0
        ///     }
        ///     
        /// </remarks>
        /// <param name="updateMarkModel">UpdateMarkModel</param>
        /// <response code="204">No content</response>
        /// todo: 400 code(bad request)
        /// todo: 500? internal error
        /// <response code="401">User is unauthorized</response>
        /// <returns>Returns NoContent</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        public async Task<ActionResult> Like(UpdateMarkModel updateMarkModel)
        {
            await Mediator.Send(new LikeCommand
            { UserId = UserId, TopicId = updateMarkModel.TopicId, IsLiked = updateMarkModel.IsLiked });
            return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult> GetMyLikes()
        {
            var result = await Mediator.Send(new GetMyLikesQuery { UserId = UserId });
            return Ok(result);
        }
    }
}
