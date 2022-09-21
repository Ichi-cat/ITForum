using ITForum.Api.Controllers;
using ITForum.Api.Models.Auth;
using ITForum.Application.Topics.Commands.CreateTopic;
using ITForum.Application.Topics.Queries.GetMyTopicListCommand;
using ITForum.Application.Topics.Queries.GetTopicDetailsByIdQuery;
using ITForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITForum.Controllers
{
    public class TopicController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult> GetTopicDetailsById(Guid id)
        {
            var topic = await Mediator.Send(new GetTopicDetailsByIdQuery { UserId = UserId, Id = id});
            return Ok(topic);
        }
        [HttpGet]
        public async  Task<ActionResult> GetTopicList(int? count=10)
        {
            var topics = await Mediator.Send(new GetMyTopicListQuery { UserId = UserId });
            return Ok(topics);
        }
        [HttpPost]
        public async Task<ActionResult> CreateTopic(CreateTopicModel model)
        {
            var id = await Mediator.Send(new CreateTopicCommand { UserId = UserId, Name = model.Name, Content = model.Content });
            return Ok(id);
        }
    }
}
