using ITForum.Application.Topics.TopicViewModels;
using ITForum.Domain.Enums;

namespace ITForum.Application.Common.Extensions
{
    internal static class SortExtension
    {
        public static IQueryable<TopicVm> Sort(this IQueryable<TopicVm> topicsQuery, TypeOfSort sort)
        {
            switch (sort)
            {
                case TypeOfSort.ByDateASC:
                    topicsQuery = topicsQuery.OrderBy(topic => topic.Created);
                    break;
                case TypeOfSort.ByDateDESC:
                    topicsQuery = topicsQuery.OrderByDescending(topic => topic.Created);
                    break;
                case TypeOfSort.ByRatingASC:
                    topicsQuery = topicsQuery.OrderByDescending(topic => topic.Marks.Where(mark => mark.IsLiked == MarkType.LIKE).Count())
                        .ThenByDescending(topic => topic.Created);
                    break;
            }
            return topicsQuery;
        }
    }
}
