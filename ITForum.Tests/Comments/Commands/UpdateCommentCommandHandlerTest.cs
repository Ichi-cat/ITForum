using ITForum.Application.Common.Exceptions;
using ITForum.Application.Comments.Commands.UpdateComment;
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
    public class UpdateCommentCommandHandlerTest:TestCommandBase
    {
        [Fact]
        public async Task UpdateCommentCommandHandler_Success()
        {
            // Arrange
            var handler = new UpdateCommentCommandHandler(Context);
            var updatedContent = "new content";

            // Act
            await handler.Handle(new UpdateCommentCommand
            {
                Id = TopicContextFactory.CommIdForUpdate,
                UserId = TopicContextFactory.UserBId,
                Content = updatedContent
            }, CancellationToken.None);

            // Assert
            Assert.NotNull(await Context.Comments.SingleOrDefaultAsync(note =>
                note.Id == TopicContextFactory.CommIdForUpdate &&
                note.Content == updatedContent));
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new UpdateCommentCommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new UpdateCommentCommand
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
            var handler = new UpdateCommentCommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateCommentCommand
                    {
                        Id = TopicContextFactory.TopicIdForUpdate,
                        UserId = TopicContextFactory.UserAId
                    },
                    CancellationToken.None);
            });
        }
    }
}
