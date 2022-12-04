using ITForum.Domain.TopicItems;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Tags.Commands.CreateTag
{
    public class CreateTagCommand : IRequest<List<Tag>>
    {
        public List<string> TagNames { get; set; }
    }
}
