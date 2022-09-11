using ITForum.Api;
using ITForum.Application.Topics.Comands.CreateTopicCommand;
using ITForum.Application.Topics.Queries.GetMyTopicListCommand;
using ITForum.Application.Topics.Queries.GetTopicDetailsByIdQuery;
using ITForum.Models;
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
            var topics = await Mediator.Send(new GetMyTopicListQuery { UserId = Guid.Empty });
            return Ok(topics);
        }
        [HttpPost]
        public async Task<ActionResult> CreateTopic(CreateTopicModel model)
        {
            var x = Mapper.Map<TestMapping>(new TestMappingSource { Id = 3, Name = "Stas", Name2 = "Err"});

            var id = await Mediator.Send(new CreateTopicCommand { UserId = Guid.Empty, Name = model.Name, Content = model.Content });
            return Ok(id);
        }

    }
}
