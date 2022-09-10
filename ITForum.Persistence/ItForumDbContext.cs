using ITForum.Application.Interfaces;
using ITForum.Domain.TopicItems;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Persistence
{
    public class ItForumDbContext : DbContext, IItForumDbContext
    {
        public DbSet<Topic> Topics { get; set; }

        public ItForumDbContext(DbContextOptions<ItForumDbContext> options)
            : base(options){
            Database.EnsureCreated();
        }
    }
}
