using ITForum.Tests.Common;
using System;
using System.Collections.Generic;
using ITForum.Application.Common.Exceptions;
using ITForum.Application.Topics.Commands.DeleteTopic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITForum.Application.Topics.Commands.CreateTopic;

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
                Id = ITForumContextFactory.TopicIdForDelete,
                UserId = ITForumContextFactory.UserAId
            }, CancellationToken.None);

            // Assert
            Assert.Null(Context.Topics.SingleOrDefault(note =>
                note.Id == ITForumContextFactory.TopicIdForDelete));
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
                        UserId = ITForumContextFactory.UserAId
                    },
                    CancellationToken.None));
        }

        [Fact]
        public async Task DeleteTopicCommandHandler_FailOnWrongUserId()
        {
            // Arrange
            var deleteHandler = new DeleteTopicCommandHandler(Context);
            var createHandler = new CreateTopicCommandHandler(Context);
            var noteId = await createHandler.Handle(
                new CreateTopicCommand
                {
                    Name = "TopicName",
                    UserId = ITForumContextFactory.UserAId
                }, CancellationToken.None);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await deleteHandler.Handle(
                    new DeleteTopicCommand
                    {
                        Id = noteId,
                        UserId = ITForumContextFactory.UserBId
                    }, CancellationToken.None));
        }
    }
}
