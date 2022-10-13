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
    public class CreateCommentCommandHandlerTest : TestCommandBase
    {
        [Fact]
        public async Task CreateCommandCommandHandler_Success()
        {
            var handler = new CreateCommentCommandHandler(Context);
            var commContent = "topic content";

            var commentId = await handler.Handle(
                new CreateCommentCommand
                {
                    Content = commContent,
                    UserId = CommandContextFactory.UserAId,
                    TopicId = TopicContextFactory.TopicIdForComment
                }, CancellationToken.None);

            var x = await Context.Comments.SingleOrDefaultAsync(comm =>
                comm.UserId == CommandContextFactory.UserAId &&
                comm.Content == commContent);
            var c = Context.Comments.ToList();
            Assert.NotNull(
                x
                );
        }
    }
}
