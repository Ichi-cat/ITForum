using ITForum.Api.Models;
using ITForum.Application.Comments.Commands.CreateComment;
using ITForum.Application.Comments.Commands.UpdateComment;
using ITForum.Application.Comments.Commands.DeleteComment;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ITForum.Application.Common.Exceptions.Generals;
using Microsoft.AspNetCore.Authorization;

namespace ITForum.Api.Controllers
{
    [Authorize]
    public class CommentController : BaseController
    {
        /// <summary>
        /// Like or dislike topic
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        /// 
        ///     Post
        ///     {
        ///         
        ///         
        ///     }
        ///     //todo: add sample
        /// </remarks>
        /// <param name="model">CreateCommentModel</param>
        /// <returns>Returns Guid</returns>
        [SwaggerResponse(201)]
        [SwaggerResponse(400, type: typeof(GeneralExceptionVm))]
        [SwaggerResponse(401)]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateComment(CreateCommentModel model)
        {
            Guid id = await Mediator.Send(new CreateCommentCommand
            {
                UserId = UserId,
                Content = model.Content,
                CommId = model.CommId,
                TopicId=model.TopicId
            });
            return Ok(id);
        }
        [SwaggerResponse(204)]
        [SwaggerResponse(400, type: typeof(GeneralExceptionVm))]
        [SwaggerResponse(401)]
        [SwaggerResponse(404, type: typeof(GeneralExceptionVm))]
        [HttpPut]
        public async Task<ActionResult> UpdateComment(UpdateCommentModel model)
        {
            var command = Mapper.Map<UpdateCommentCommand>(model);
            command.UserId = UserId;
            MediatR.Unit id = await Mediator.Send(command); 
            return NoContent();
        }
        [SwaggerResponse(204)]
        [SwaggerResponse(400, type: typeof(GeneralExceptionVm))]
        [SwaggerResponse(401)]
        [SwaggerResponse(404, type: typeof(GeneralExceptionVm))]
        [HttpDelete]
        public async Task<ActionResult> DeleteComment(Guid id)
        {
            var command = new DeleteCommentCommand
            {
                UserId = UserId,
                Id = id
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
