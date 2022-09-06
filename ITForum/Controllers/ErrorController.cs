using ITForum.Domain.Errors;
using ITForum.Domain.Topic;
using Microsoft.AspNetCore.Mvc;

namespace ITForum.Controllers
{
    public class ErrorController : BaseController
    {
        [HttpGet]
        public ActionResult Error_404()
        {
            return NotFound(Errors.NotFound_404);
        }
        [HttpGet]
        public ActionResult GetTopicList(int? count=5)
        {
            //send to cqrs
            return Ok(new List<Topic> { new Topic { Name="Ivan"}, new Topic { Name="Mihail" }, new Topic { Name = count.ToString() } });
        }
    }
}
