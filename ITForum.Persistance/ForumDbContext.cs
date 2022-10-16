using Microsoft.EntityFrameworkCore;
using ITForum.Domain.TopicItems;
using ITForum.Application.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ITForum.Domain.ItForumUser;

namespace ITForum.Persistance
{
    public class ItForumDbContext: IdentityDbContext<ItForumUser, ItForumRole, Guid>, IItForumDbContext
    {
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public ItForumDbContext(DbContextOptions<ItForumDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}