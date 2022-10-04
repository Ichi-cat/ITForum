using ITForum.Application.Comments.Commands.CreateComment;
using ITForum.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ITForum.Tests.Comments.Commands
{
    public class CreateCommentCommandHandlerTest:TestCommandBase
    {
        [Fact]
        public async Task CreateCommandCommandHandler_Success()
        {
            var handler = new CreateCommentCommandHandler(Context);
            var topicName = "topic name";
            var topicContent = "topic content";

            var topicId = await handler.Handle(
                new CreateCommentCommand
                {
                    Content = topicContent,
                    UserId = TopicContextFactory.UserAId,
                    TopicId = Context.Topics.FirstOrDefault().Id
                }, CancellationToken.None);

            Assert.NotNull(
                await Context.Topics.SingleOrDefaultAsync(topic =>
                topic.UserId == TopicContextFactory.UserAId && topic.Name == topicName &&
                topic.Content == topicContent)
                );
        }
    }
}
