using ITForum.Application.Topic.Comands.CreateTopicCommand;
using ITForum.Application.Topic.Comands.GetTopicListCommand;
using ITForum.Domain.Topic;
using ITForum.Models;
using Microsoft.AspNetCore.Mvc;

namespace ITForum.Controllers
{
    public class TopicController : BaseController
    {
        [HttpGet("{id}")]
        public ActionResult GetTopicDetailsById(Guid id)
        {
            return Ok(new { id = id });
        }
        [HttpGet]
        public ActionResult GetTopicList(int? count=10)
        {
            Mediator.Send(new GetTopicListCommand { UserId = Guid.Empty });
            return Ok(new List<Topic> { new Topic { Name="Ivan"}, new Topic { Name="Mihail" }, new Topic { Name = count.ToString() } });
        }
        [HttpPost]
        public async Task<ActionResult> CreateTopic(CreateTopicModel model)
        {
            await Mediator.Send(new CreateTopicCommand { UserId = Guid.Empty });
            return Ok();
        }

    }
}
