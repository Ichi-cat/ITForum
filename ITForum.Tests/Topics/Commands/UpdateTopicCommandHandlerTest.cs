using ITForum.Application.Common.Exceptions;
using ITForum.Tests.Common;
using ITForum.Application.Topics.Commands.UpdateTopic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ITForum.Tests.Topics.Commands
{
    public class UpdateTopicCommandHandlerTest:TestCommandBase
    {
        [Fact]
        public async Task UpdateNoteCommandHandler_Success()
        {
            // Arrange
            var handler = new UpdateTopicCommandHandler(Context);
            var updatedName = "new name";

            // Act
            await handler.Handle(new UpdateTopicCommand
            {
                Id = TopicContextFactory.TopicIdForUpdate,
                UserId = TopicContextFactory.UserBId,
                Name = updatedName
            }, CancellationToken.None);

            // Assert
            Assert.NotNull(await Context.Topics.SingleOrDefaultAsync(note =>
                note.Id == TopicContextFactory.TopicIdForUpdate &&
                note.Name == updatedName));
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new UpdateTopicCommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new UpdateTopicCommand
                    {
                        Id = Guid.NewGuid(),
                        UserId = TopicContextFactory.UserAId
                    },
                    CancellationToken.None));
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_FailOnWrongUserId()
        {
            // Arrange
            var handler = new UpdateTopicCommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateTopicCommand
                    {
                        Id = TopicContextFactory.TopicIdForUpdate,
                        UserId = TopicContextFactory.UserAId
                    },
                    CancellationToken.None);
            });
        }
    }
}
