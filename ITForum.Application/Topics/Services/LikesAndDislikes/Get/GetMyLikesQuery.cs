using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Application.Topics.Services.LikesAndDislikes.Get
{
    public class GetMyLikesQuery : IRequest<MarkListVM>
    {
        public Guid UserId { get; set; }
    }
}
