using ITForum.Domain.TopicItems;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<Attachment> Attachments { get; set; }
    }

}
