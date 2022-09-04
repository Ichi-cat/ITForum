using Microsoft.AspNetCore.Mvc;

namespace ITForum.Controllers
{
    public class TopicController : BaseController
    {
        [HttpGet]
        public string Get()
        {
            return "Hello from controller";
        }
    }
}
