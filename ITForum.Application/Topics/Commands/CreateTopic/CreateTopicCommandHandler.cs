﻿using ITForum.Application.Interfaces;
using ITForum.Domain.TopicItems;
using MediatR;


namespace ITForum.Application.Topics.Commands.CreateTopic
{
    /// <summary>
    /// Логика создания топика
    /// </summary>
    public class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, Guid>
    {
        private readonly IItForumDbContext _context;

        public CreateTopicCommandHandler(IItForumDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = new Topic
            {
                Name = request.Name,
                Content = request.Content,
                Attachments = request.Attachments,
                Id = Guid.NewGuid(),
                UserId = request.UserId
            };
            await _context.Topics.AddAsync(topic);
            await _context.SaveChangesAsync();
            return topic.Id;
        }
    }
}