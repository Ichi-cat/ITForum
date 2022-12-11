using ITForum.Api.Models;
using ITForum.Application.Topics.Commands.CreateTopic;
using ITForum.Application.Topics.Commands.DeleteTopic;
using ITForum.Application.Topics.Commands.UpdateTopic;
using ITForum.Application.Topics.Queries.GetTopicDetailsById;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ITForum.Application.Common.Exceptions.Generals;
using ITForum.Application.Interfaces;
using ITForum.Application.Topics.Commands.UploadAttachments;
using ITForum.Application.Topics.Queries.GetTopicListByTag;
using ITForum.Application.Topics.Queries.GetTopicList;
using ITForum.Application.Topics.TopicViewModels;
using Microsoft.AspNetCore.Authorization;
using ITForum.Application.Topics.Queries.GetTopicsBySubscriptions;
using ITForum.Application.Topics.Queries.GetTopicListByUser;
using ITForum.Application.Common.Exceptions;

namespace ITForum.Api.Controllers
{
    [Authorize]
    public class TopicController : BaseController
    {
        readonly IBufferedFileUploadService _bufferedFileUploadService;
        public TopicController(IBufferedFileUploadService bufferedFileUploadService)
        {
            _bufferedFileUploadService = bufferedFileUploadService;
        }
        /// <summary>
        /// Get topic list by tag
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        /// 
        ///     Get
        ///     /tag?string=string
        ///     
        /// </remarks>
        /// <param name="count">Int32</param>
        /// <returns>Returns TopicListVm</returns>
        [SwaggerResponse(200, type: typeof(TopicListVm))]
        [SwaggerResponse(400, type: typeof(GeneralExceptionVm))]
        [SwaggerResponse(401)]
        [HttpGet("ByTag")]
        public async Task<ActionResult<TopicListVm>> GetTopicListByTag([FromQuery] ShowTopicsModel showTopicsModel, [FromQuery] PaginationModel pagination, string tagName)
        {
            var topics = await Mediator.Send(new GetTopicListByTagQuery { TagName = tagName, Page = pagination.Page, PageSize = pagination.PageSize, Sort = showTopicsModel.Sort });
            return Ok(topics);
        }
        /// <summary>
        /// Get topic details by id
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        /// 
        ///     Get
        ///     /topic/27316C01-6967-48F1-AD9F-FFDBD940647C
        ///     
        /// </remarks>
        /// <param name="id">Guid</param>
        /// <returns>Returns TopicDetailsVm</returns>
        [SwaggerResponse(200, type: typeof(TopicDetailsVm))]
        [SwaggerResponse(400, type: typeof(GeneralExceptionVm))]
        [SwaggerResponse(401)]
        [SwaggerResponse(404, type: typeof(GeneralExceptionVm))]
        [HttpGet("{id}")]
        public async Task<ActionResult<TopicDetailsVm>> GetTopicDetailsById(Guid id)
        {
            var topic = await Mediator.Send(new GetTopicDetailsByIdQuery { Id = id , UserId = UserId});
            return Ok(topic);
        }
        //todo: cencrete topiclist
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
        [SwaggerResponse(200, type: typeof(Guid))]
        [SwaggerResponse(400, type: typeof(GeneralExceptionVm))]
        [SwaggerResponse(401)]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateTopic(CreateTopicModel model)
        {
            var id = await Mediator.Send(new CreateTopicCommand
            { UserId = UserId, Name = model.Name, Content = model.Content, AttachmentsId = model.AttachmentsId, Tags = model.TagsNames });
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
        [SwaggerResponse(204)]
        [SwaggerResponse(400, type: typeof(GeneralExceptionVm))]
        [SwaggerResponse(401)]
        [SwaggerResponse(404, type: typeof(GeneralExceptionVm))]
        [HttpPut]
        public async Task<ActionResult> UpdateTopic(UpdateTopicModel updateTopicModel)
        {
            await Mediator.Send(new UpdateTopicCommand
            { UserId = UserId, Id = updateTopicModel.Id, Name = updateTopicModel.Name,
                Content = updateTopicModel.Content, AttachmentsId = updateTopicModel.AttachmentsId });
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
        /// <returns>Returns NoContent</returns>
        [SwaggerResponse(204)]
        [SwaggerResponse(400, type: typeof(GeneralExceptionVm))]
        [SwaggerResponse(401)]
        [SwaggerResponse(404, type: typeof(GeneralExceptionVm))]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTopic(Guid id)
        {
            await Mediator.Send(new DeleteTopicCommand { UserId = UserId, Id = id });
            return NoContent();
        }
        /// <summary>
        /// Upload attachments on server
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<ActionResult<List<Guid>>> UploadAttachmentsOnServer(IFormFile[] files)
        {
            var resultUrl = await _bufferedFileUploadService.UploadFiles(files);
            var id = await Mediator.Send(new UploadAttachmentsCommand { AttachmentsUrl = resultUrl, UserId = UserId });
            // TODO: attach to topic
            return Ok(id);
        }
        /// <summary>
        /// Get topic list(main page)
        /// </summary>
        /// ///<remarks>
        ///     case 0 - SortByDateASC 
        ///     
        ///     case 1 - SortByDateDESC 
        ///     
        ///     case 2 - SortByRatingASC
        /// </remarks>
        /// <param name="TypeOfSort"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TopicListVm>>> GetTopicList([FromQuery] ShowTopicsModel showTopicsModel, [FromQuery] PaginationModel pagination)
        {
            var topics = await Mediator.Send(new GetTopicListQuery { Page = pagination.Page, PageSize = pagination.PageSize, Sort = showTopicsModel.Sort });
            return Ok(topics);
        }

        [HttpGet("BySubscriptions")]
        public async Task<ActionResult> GetTopicsBySubscriptions([FromQuery]PaginationModel paginationModel, [FromQuery]ShowTopicsModel sortModel)
        {
            var topics = await Mediator.Send(new GetTopicsBySubscriptionsQuery { UserId = UserId, Page= paginationModel.Page, PageSize= paginationModel.PageSize, Sort=sortModel.Sort });
            return Ok(topics);
        }
        /// <summary>
        /// Get topic list by UserId
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("ByUserId")]
        public async Task<ActionResult<IEnumerable<TopicListVm>>> GetTopicListByUserId([FromQuery]GetTopicListByUserQuery query)
        {
            if (query.UserId == null && UserId != Guid.Empty)
            {
                query.UserId = UserId;
            }
            if (query.UserId == null)
            {
                throw new UnauthorizeException();
            }
            var topics = await Mediator.Send(query);
            return Ok(topics);
        }
    }
}
