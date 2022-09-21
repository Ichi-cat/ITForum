using ITForum.Api.Models;
using ITForum.Application.Comments.Commands.CreateComment;
using ITForum.Application.Comments.Commands.UpdateComment;
using ITForum.Application.Comments.Commands.DeleteComment;
using ITForum.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITForum.Api.Controllers
{
    public class CommentController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult> CreateComment(CrUDCommentModel model)
        {
            Guid id = await Mediator.Send(new CreateCommentCommand
            {
                UserId = Guid.Empty,
                Content = model.Content,
                Comm = model.Comm,
                Topic = model.Topic
            });
            return Ok(id);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateComment(CrUDCommentModel model)
        {
            var command = Mapper.Map<UpdateCommentCommand>(model);
            command.UserId = UserId;
            MediatR.Unit id = await Mediator.Send(command);
            return NoContent();
        }
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
