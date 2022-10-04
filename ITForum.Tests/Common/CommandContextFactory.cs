using ITForum.Domain.TopicItems;
using ITForum.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Tests.Common
{
    public class CommandContextFactory
    {
        public static Guid UserAId = Guid.NewGuid();
        public static Guid UserBId = Guid.NewGuid();
        public static Guid CommIdForDelete = Guid.NewGuid();
        public static Guid CommIdForUpdate = Guid.NewGuid();
        public static ItForumDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ItForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ItForumDbContext(options);
            context.Database.EnsureCreated();
            context.Topics.AddRange(
                new Topic
                {
                    Id = Guid.Parse("A6BB65BB-5AC2-4AFA-8A28-2616F675B825"),
                    UserId = UserAId,
                    Name = "Name1",
                    Content = "Context1",
                    Attachments = { },
                    Comment = { },
                    Created = DateTime.Now
                });
            context.Comments.AddRange(
                new Comment
                {
                    Id = Guid.Parse("A6BB65BB-5AC2-4AFA-8A28-2616F675B825"),
                    UserId = UserAId,
                    Content = "Context1",
                    TopicId = Guid.Parse("A6BB65BB - 5AC2 - 4AFA - 8A28 - 2616F675B825"),
                },
                new Comment
                {
                    Id = Guid.Parse("909F7C29-891B-4BE1-8504-21F84F262084"),
                    UserId = UserBId,
                    Content = "Content2",
                    TopicId = Guid.Parse("A6BB65BB - 5AC2 - 4AFA - 8A28 - 2616F675B825")
                },
                new Comment
                {
                    UserId = UserAId,
                    Id = CommIdForDelete,
                    Content = "Content3",
                    CommId= Guid.Parse("A6BB65BB-5AC2-4AFA-8A28-2616F675B825")
                },
                new Comment
                {

                    UserId = UserBId,
                    Id = CommIdForUpdate,
                    Content = "Content4",
                    CommId= Guid.Parse("909F7C29-891B-4BE1-8504-21F84F262084")
                }
            ) ;
            context.SaveChanges();
            return context;
        }
        public static void Destroy(ItForumDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
