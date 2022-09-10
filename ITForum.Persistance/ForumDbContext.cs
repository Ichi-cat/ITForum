using Microsoft.EntityFrameworkCore;
using ITForum.Domain.TopicItems;
using ITForum.Application.Interfaces;

namespace ITForum.Persistance
{
    public class ITForumDbContext:DbContext, IItForumDbContext
    {
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public ITForumDbContext(DbContextOptions<ITForumDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}