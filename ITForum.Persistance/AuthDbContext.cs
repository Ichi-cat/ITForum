using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ITForum.Domain.ItForumUser;
using ITForum.Application.Interfaces;

namespace ITForum.Persistance
{
    public class AuthDbContext: IdentityDbContext<ItForumUser, ItForumRole, Guid>, IAuthDbContext
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ItForumUser>().HasMany(u => u.Subscriptions).WithMany(u => u.Subscribers).UsingEntity(j => j.ToTable("UserSubscriptions"));
        }
    }
}