using Microsoft.AspNetCore.Mvc;

namespace cuppyzh.xtrtools.poadocumentgenerator.Controllers
{
    [ApiController]
    [Route("api/v1/pr-changes")]
    public class PullRequestApiController: ControllerBase
    {
        [HttpPost("Get")]
        public IActionResult Get(string prurl)
        {
            return Ok();
        }
    }
}
