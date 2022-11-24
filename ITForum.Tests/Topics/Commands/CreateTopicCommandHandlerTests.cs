using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ITForum.Application.Topics.Commands.CreateTopic;
using ITForum.Tests.Common;
using Xunit;

namespace ITForum.Tests.Topics.Commands
{
    public class CreateTopicCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateTopicCommandHandler_Success()
        {
            //var handler = new CreateTopicCommandHandler(Context);
            //var topicName = "topic name";
            //var topicContent = "topic content";

            //var topicId = await handler.Handle(
            //    new CreateTopicCommand
            //    {
            //        Name = topicName,
            //        Content = topicContent,
            //        UserId = TopicContextFactory.UserAId
            //    }, CancellationToken.None);

            //Assert.NotNull(
            //    await Context.Topics.SingleOrDefaultAsync(topic =>
            //    topic.UserId == TopicContextFactory.UserAId && topic.Name == topicName &&
            //    topic.Content == topicContent)
            //    );
        }
    }
}
