using ITForum.Domain.TopicItems;
using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Interfaces
{
    public interface IItForumDbContext
    {
        DbSet<Attachment> Attachments { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Mark> Marks { get; set; }
        DbSet<Topic> Topics { get; set; }
        DbSet<Tag> Tags { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
