using Microsoft.EntityFrameworkCore;

namespace ITForum.Application.Common.Extensions
{
    internal static class PaginationExtension
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
        public static async Task<int> GetPageCount<T>(this IQueryable<T> query, int pageSize)
        {
            int count = await query.CountAsync();
            int pageCount = (int)Math.Ceiling((double)count / pageSize);
            return pageCount;
        }
    }
}
