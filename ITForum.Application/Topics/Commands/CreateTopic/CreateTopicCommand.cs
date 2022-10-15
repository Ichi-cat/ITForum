﻿using ITForum.Domain.TopicItems;
using MediatR;

namespace ITForum.Application.Topics.Commands.CreateTopic
{
    /// <summary>
    /// Класс содержит информацию о том что необходимо для создания топика
    /// </summary>
    public class CreateTopicCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Guid AttachmentId { get; set; }
        public List<Guid> AttachmentsUrl { get; set; }
    }
}
