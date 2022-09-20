using ITForum.Api.Models;
using ITForum.Application.Comments.Commands.CreateComment;
using ITForum.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITForum.Api.Controllers
{
    public class CommentController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult> CreateComment(CreateCommentModel model)
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
