using ITForum.Api.Models;
using ITForum.Application.Topics.Services.LikesAndDislikes;
using ITForum.Application.Topics.Services.LikesAndDislikes.Get;
using ITForum.Application.Topics.Services.LikesAndDislikes.GetTopicLikesQuery;
using ITForum.Controllers;
using ITForum.Domain.Errors.Generals;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        /// <returns>Returns NoContent</returns>
        [SwaggerResponse(204)]
        [SwaggerResponse(400, type: typeof(GeneralExceptionVm))]
        [SwaggerResponse(401)]
        [SwaggerResponse(404, type: typeof(GeneralExceptionVm))]
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
        [HttpGet("{id}")]
        public async Task<ActionResult> GetTopicLikes(Guid topicId)
        {
            var result = await Mediator.Send(new GetTopicLikesQuery { UserId = UserId, TopicId = topicId});
            return Ok(result);
        }
    }
}
