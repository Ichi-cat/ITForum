using ITForum.Domain.TopicItems;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Interfaces
{
    public interface IItForumDbContext
    {
        DbSet<Topic> Topics { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
