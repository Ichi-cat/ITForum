using MediatR;

namespace ITForum.Application.Topics.Queries.GetTopicListQuery
{
    public class GetTopicListQuery:IRequest<TopicListVM>
    {
        public int Start { get; set; }
        public int Tail { get; set; }
    }
}
