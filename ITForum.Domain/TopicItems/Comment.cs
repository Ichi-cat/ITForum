namespace ITForum.Domain.TopicItems
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public Topic? Topic { get; set; } 
        public Guid? TopicId { get; set; } 
        public Comment? Comm { get; set; }    //оно точно надо? 
        public Guid? CommId { get; set; }
        public List<Mark> Marks { get; set; }
    }
}
