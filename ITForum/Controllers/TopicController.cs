using ITForum.Api.Models;
using ITForum.Application.Topics.Commands.CreateTopic;
using ITForum.Application.Topics.Commands.DeleteTopic;
using ITForum.Application.Topics.Commands.UpdateTopic;
using ITForum.Api.Controllers;
using ITForum.Api.Models.Auth;
using ITForum.Application.Topics.Queries.GetMyTopicListCommand;
using ITForum.Application.Topics.Queries.GetTopicDetailsByIdQuery;
using ITForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITForum.Application.Topics.Services.LikesAndDislikes;
using ITForum.Domain.TopicItems;

namespace ITForum.Controllers
{
    public class TopicController : BaseController
    {
        /// <summary>
        /// Get topic by id
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        /// 
        ///     Get
        ///     /topic/27316C01-6967-48F1-AD9F-FFDBD940647C
        ///     
        /// </remarks>
        /// <param name="id">Guid</param>
        /// <response code="200">Success</response>
        /// todo: 400 code(bad request)
        /// todo: 500? internal error
        /// <response code="401">User is unauthorized</response>
        /// <returns>Returns TopicDetailsVm</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<ActionResult<TopicDetailsVm>> GetTopicDetailsById(Guid id)
        {
            var topic = await Mediator.Send(new GetTopicDetailsByIdQuery { UserId = UserId, Id = id });
            return Ok(topic);
        }
        //todo: cencrete topiclist
        /// <summary>
        /// Get topic list
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        /// 
        ///     Get
        ///     /topic?count=10
        ///     
        /// </remarks>
        /// <param name="count">Int32</param>
        /// <response code="200">Success</response>
        /// todo: 400 code(bad request)
        /// todo: 500? internal error
        /// <response code="401">User is unauthorized</response>
        /// <returns>Returns TopicListVm</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<ActionResult<TopicListVm>> GetTopicList(int? count = 10)
        {
            var topics = await Mediator.Send(new GetMyTopicListQuery { UserId = UserId });
            return Ok(topics);
        }
        /// <summary>
        /// Create topic
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        /// 
        ///     Post
        ///     {
        ///         "name": "Name of topic",
        ///         "content": "Content of topic"
        ///     }
        ///     
        /// </remarks>
        /// <param name="model">CreateTopicModel</param>
        /// <response code="200">Success</response>
        /// todo: 400 code(bad request)
        /// todo: 500? internal error
        /// <response code="401">User is unauthorized</response>
        /// <returns>Returns Guid</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateTopic(CreateTopicModel model)
        {
            var id = await Mediator.Send(new CreateTopicCommand
            { UserId = Guid.Empty, Name = model.Name, Content = model.Content });
            return Ok(id);
        }
        /// <summary>
        /// Update topic
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        /// 
        ///     Put
        ///     {
        ///         "id": "3fa85f44-5717-4561-b3fc-2c963f66afa6",
        ///         "name": "Topic Name",
        ///         "content": "Topic content"
        ///     }
        ///     
        /// </remarks>
        /// <param name="updateTopicModel">UpdateTopicModel</param>
        /// <response code="204">No content</response>
        /// todo: 400 code(bad request)
        /// todo: 500? internal error
        /// <response code="401">User is unauthorized</response>
        /// <returns>Returns NoContent</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        public async Task<ActionResult> UpdateTopic(UpdateTopicModel updateTopicModel)
        {
            await Mediator.Send(new UpdateTopicCommand
            { UserId = Guid.Empty, Id = updateTopicModel.Id, Name = updateTopicModel.Name, Content = updateTopicModel.Content });
            return NoContent();
        }
        /// <summary>
        /// Delete topic by id
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        /// 
        ///     Delete
        ///     /topic/27316C01-6967-48F1-AD9F-FFDBD940647C
        ///     
        /// </remarks>
        /// <param name="id">Guid</param>
        /// <response code="204">No content</response>
        /// todo: 400 code(bad request)
        /// todo: 500? internal error
        /// <response code="401">User is unauthorized</response>
        /// <returns>Returns NoContent</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTopic(Guid id)
        {
            await Mediator.Send(new DeleteTopicCommand { UserId = UserId, Id = id });
            return NoContent();
        }
    }
}
