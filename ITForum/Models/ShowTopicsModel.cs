using ITForum.Domain.Enums;

namespace ITForum.Api.Models
{
    public class ShowTopicsModel
    {
        public TypeOfSort Sort { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
