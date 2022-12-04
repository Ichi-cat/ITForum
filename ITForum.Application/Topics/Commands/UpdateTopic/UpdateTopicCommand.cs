using MediatR;

namespace ITForum.Application.Topics.Commands.UpdateTopic
{
    /// <summary>
    /// Класс содержит информацию о том что необходимо для обновления топика
    /// </summary>
    public class UpdateTopicCommand : IRequest
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }  
        public string Name { get; set; }
        public string Content { get; set; }
        public List<Guid> AttachmentsId { get; set; }
    }
}
