using Microsoft.EntityFrameworkCore;
using ITForum.Domain.Topic;

namespace ITForum.Persistance
{
    public class ITForumDbContext:DbContext
    {
        DbSet<Attachment> Attachments { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Mark> Marks { get; set; }
        DbSet<Topic> Topic { get; set; }
        public ITForumDbContext(DbContextOptions<ITForumDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}