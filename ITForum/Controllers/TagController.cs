using ITForum.Api.Models;
using ITForum.Application.Common.Exceptions.Generals;
using ITForum.Application.Tags.Commands.CreateTag;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ITForum.Api.Controllers
{
    public class TagController : BaseController
    {
        [SwaggerResponse(201)]
        [SwaggerResponse(400, type: typeof(GeneralExceptionVm))]
        [SwaggerResponse(401)]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateTag(CreateTagModel model)
        {
            Guid id = await Mediator.Send(new CreateTagCommand
            {
                Name = model.Name
            });
            return Ok(id);
        }
    }
}
