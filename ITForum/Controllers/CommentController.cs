using ITForum.Api.Models;
using ITForum.Application.Comments.Commands.CreateComment;
using ITForum.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITForum.Api.Controllers
{
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
        /// <response code="200">Success</response>
        /// todo: 400 code(bad request)
        /// todo: 500? internal error
        /// <response code="401">User is unauthorized</response>
        /// <returns>Returns Guid</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateComment(CreateCommentModel model)
        {
            var id = await Mediator.Send(new CreateCommentCommand
            {
                UserId = Guid.Empty,
                Content = model.Content,
                Comm = model.Comm,
                Topic = model.Topic
            });
            return Ok(id);
        }
    }
}
