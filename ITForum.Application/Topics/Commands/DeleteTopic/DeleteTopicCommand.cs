using System;
using MediatR;

namespace ITForum.Application.Topics.Commands.DeleteTopic
{
    /// <summary>
    /// Класс содержит информацию о том что необходимо чтобы удалить топик
    /// </summary>
    public class DeleteTopicCommand : IRequest
    {
        public  Guid UserId { get; set; }
        public Guid Id { get; set; }
    }
}
