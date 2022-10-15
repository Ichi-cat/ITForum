using ITForum.Api.Models;
using ITForum.Controllers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ITForum.Application.Common.Exceptions.Generals;
using ITForum.Application.Marks.Commands.SetMark;
using ITForum.Application.Marks.Queries.GetTopicLikesCountQuery;

namespace ITForum.Api.Controllers
{
    public class MarkController : BaseController
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
            await Mediator.Send(new SetMarkCommand
            { UserId = UserId, TopicId = updateMarkModel.TopicId, IsLiked = updateMarkModel.IsLiked });
            return NoContent();
        }
        /// <summary>
        /// Get topic likes count
        /// </summary>
        /// ///<remarks>
        /// Sample request:
        /// 
        ///     Put
        ///     {
        ///         "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "isLiked": 0
        ///     }
        ///     
        /// ///</remarks>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpGet("{topicId}")]
        public async Task<ActionResult> GetTopicLikes(Guid topicId)
        {
            var result = await Mediator.Send(new GetTopicLikesCountQuery { UserId = UserId, TopicId = topicId});
            return Ok(result);
        }
    }
}
