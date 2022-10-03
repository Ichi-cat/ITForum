using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITForum.Domain.TopicItems;
using ITForum.Persistance;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Tests.Common
{
    public class ITForumContextFactory
    {
        public static Guid UserAId = Guid.NewGuid();
        public static Guid UserBId = Guid.NewGuid();
        public static Guid TopicIdForDelete = Guid.NewGuid();
        public static Guid TopicIdForUpdate = Guid.NewGuid();
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
                },
                new Topic
                {
                    Id = Guid.Parse("909F7C29-891B-4BE1-8504-21F84F262084"),
                    UserId = UserBId,
                    Name = "Name2",
                    Content = "Content2",
                    Attachments = { },
                    Comment = { },
                    Created = DateTime.Now
                },
                new Topic
                {
                    UserId = UserAId,
                    Id = TopicIdForDelete,
                    Name = "Name3",
                    Content= "Content3",
                    Attachments = { },
                    Comment = { },
                    Created = DateTime.Now

                },
                new Topic
                {

                    UserId = UserBId,
                    Id = TopicIdForUpdate,
                    Name = "Name4",
                    Content="Content4",
                    Attachments = { },
                    Comment = { },
                    Created = DateTime.Now
                }
            );
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
