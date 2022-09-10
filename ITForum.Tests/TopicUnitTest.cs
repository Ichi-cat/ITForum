using ITForum.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Tests
{
    internal class TopicUnitTest
    {
        public static Guid UserAId = Guid.NewGuid();
        public static Guid UserBId = Guid.NewGuid();

        public static Guid NoteIdForDelete = Guid.NewGuid();

        public static ItForumDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ItForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new ItForumDbContext(options);
            context.Database.EnsureCreated();
            context.AddRange();
            context.SaveChanges();
            return context;
        }

        internal static void Destroy(ItForumDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
