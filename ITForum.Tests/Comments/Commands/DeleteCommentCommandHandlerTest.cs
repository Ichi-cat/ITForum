using ITForum.Application.Common.Exceptions;
using ITForum.Application.Comments.Commands.DeleteComment;
using ITForum.Tests.Common;
using Xunit;
using ITForum.Application.Comments.Commands.CreateComment;

namespace ITForum.Tests.Comments.Commands
{
    public class DeleteCommentCommandHandlerTest:TestCommandBase
    {
        [Fact]
        public async Task DeleteCommentCommandHandler_Success()
        {
            // Arrange
            var handler = new DeleteCommentCommandHandler(Context);

            // Act
            await handler.Handle(new DeleteCommentCommand
            {
                Id = TopicContextFactory.CommIdForDelete,
                UserId = TopicContextFactory.UserAId
            }, CancellationToken.None);

            // Assert
            Assert.Null(Context.Topics.SingleOrDefault(note =>
                note.Id == TopicContextFactory.CommIdForDelete));
        }

        [Fact]
        public async Task DeleteTopicCommandHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new DeleteCommentCommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new DeleteCommentCommand
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
            var deleteHandler = new DeleteCommentCommandHandler(Context);
            var createHandler = new CreateCommentCommandHandler(Context);
            var noteId = await createHandler.Handle(
                new CreateCommentCommand
                {
                    Content = "TopicName",
                    UserId = TopicContextFactory.UserAId
                }, CancellationToken.None);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await deleteHandler.Handle(
                    new DeleteCommentCommand
                    {
                        Id = noteId,
                        UserId = TopicContextFactory.UserBId
                    }, CancellationToken.None));
        }
    }
}
