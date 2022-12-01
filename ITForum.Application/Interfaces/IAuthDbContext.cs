using ITForum.Domain.ItForumUser;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Interfaces
{
    public interface IAuthDbContext
    {
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<ItForumUser> Users { get; set; }
        DbSet<ItForumRole> Roles { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
