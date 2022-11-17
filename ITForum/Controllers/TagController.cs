using ITForum.Api.Models;
using ITForum.Application.Common.Exceptions.Generals;
using ITForum.Application.Tags.Commands.CreateTag;
using ITForum.Application.Tags.Queries.GetTags;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ITForum.Api.Controllers
{
    public class TagController : BaseController
    {
        /// <summary>
        /// Get list of tags
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<TagListVM>> GetTags([FromQuery] PaginationModel pagination)
        {
           var tags = await Mediator.Send(new GetTagsListQuery { Page = pagination.Page , PageSize = pagination.PageSize});
            return Ok(tags);
        }
    }
}
