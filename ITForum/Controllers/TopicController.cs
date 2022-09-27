using ITForum.Api.Models;
using ITForum.Application.Topics.Commands.CreateTopic;
using ITForum.Application.Topics.Commands.DeleteTopic;
using ITForum.Application.Topics.Commands.UpdateTopic;
using ITForum.Api.Controllers;
using ITForum.Api.Models.Auth;
using ITForum.Application.Topics.Queries.GetMyTopicListCommand;
using ITForum.Application.Topics.Queries.GetTopicDetailsByIdQuery;
using ITForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITForum.Application.Topics.Services.LikesAndDislikes;
using ITForum.Domain.TopicItems;

namespace ITForum.Controllers
{
    public class TopicController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult> GetTopicDetailsById(Guid id)
        {
            var topic = await Mediator.Send(new GetTopicDetailsByIdQuery { UserId = UserId, Id = id });
            return Ok(topic);
        }
        [HttpGet]
        public async Task<ActionResult> GetTopicList(int? count = 10)
        {
            var topics = await Mediator.Send(new GetMyTopicListQuery { UserId = UserId });
            return Ok(topics);
        }
        [HttpPost]
        public async Task<ActionResult> CreateTopic(CreateTopicModel model)
        {
            var id = await Mediator.Send(new CreateTopicCommand
            { UserId = Guid.Empty, Name = model.Name, Content = model.Content });
            return Ok(id);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateTopic(UpdateTopicModel updateTopicModel)
        {
            await Mediator.Send(new UpdateTopicCommand
            { UserId = Guid.Empty, Id = updateTopicModel.Id, Name = updateTopicModel.Name, Content = updateTopicModel.Content });
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTopic(Guid id)
        {
            await Mediator.Send(new DeleteTopicCommand { UserId = UserId, Id = id });
            return NoContent();
        }
        [HttpPatch]
        public async Task<ActionResult> Like(UpdateMarkModel updateMarkModel)
        {
            await Mediator.Send(new LikeCommand
            { UserId = UserId, TopicId = updateMarkModel.TopicId, IsLiked = updateMarkModel.IsLiked });
            return NoContent();
        }
    }
}
