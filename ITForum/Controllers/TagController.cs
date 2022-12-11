using ITForum.Api.Models;
using ITForum.Application.Tags.Queries.GetTags;
using ITForum.Application.Tags.TagsViewModel;
using ITForum.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITForum.Api.Controllers
{
    public class TagController : BaseController
    {
        /// <summary>
        /// Get list of tags
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<TagListVM>> GetTags([FromQuery] PaginationModel pagination, TagSort sort)
        {
           var tags = await Mediator.Send(new GetTagsListQuery { Page = pagination.Page , PageSize = pagination.PageSize, Sort=sort});
            return Ok(tags);
        }
    }
}
