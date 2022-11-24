using ITForum.Tests.Common;
using System;
using System.Collections.Generic;
using ITForum.Application.Common.Exceptions;
using ITForum.Application.Topics.Commands.DeleteTopic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITForum.Application.Topics.Commands.CreateTopic;
using Xunit;

namespace ITForum.Tests.Topics.Commands
{
    public class DeleteTopicCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteTopicCommandHandler_Success()
        {
            // Arrange
            var handler = new DeleteTopicCommandHandler(Context);

            // Act
            await handler.Handle(new DeleteTopicCommand
            {
                Id = TopicContextFactory.TopicIdForDelete,
                UserId = TopicContextFactory.UserAId
            }, CancellationToken.None);

            // Assert
            Assert.Null(Context.Topics.SingleOrDefault(note =>
                note.Id == TopicContextFactory.TopicIdForDelete));
        }

        [Fact]
        public async Task DeleteTopicCommandHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new DeleteTopicCommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new DeleteTopicCommand
                    {
                        Id = Guid.NewGuid(),
                        UserId = TopicContextFactory.UserAId
                    },
                    CancellationToken.None));
        }

        [Fact]
        public async Task DeleteTopicCommandHandler_FailOnWrongUserId()
        {
            // Arrange
            var deleteHandler = new DeleteTopicCommandHandler(Context);
            //var createHandler = new CreateTopicCommandHandler(Context);
            //var noteId = await createHandler.Handle(
            //    new CreateTopicCommand
            //    {
            //        Name = "TopicName",
            //        UserId = TopicContextFactory.UserAId
            //    }, CancellationToken.None);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await deleteHandler.Handle(
                    new DeleteTopicCommand
                    {
                        Id = TopicContextFactory.TopicIdForDelete,
                        UserId = TopicContextFactory.UserBId
                    }, CancellationToken.None));
        }
    }
}
