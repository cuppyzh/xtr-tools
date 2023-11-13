using cuppyzh.xtrtools.poadocumentgenerator.Services;
using Microsoft.AspNetCore.Mvc;

namespace cuppyzh.xtrtools.poadocumentgenerator.Controllers
{
    [ApiController]
    [Route("api/v1/pr-changes")]
    public class PullRequestApiController: ControllerBase
    {
        private readonly PrChangesServices _services = new PrChangesServices();

        [HttpPost("Get")]
        public IActionResult Get([FromForm] PullRequestApiGetRequestBody request)
        {

            return Ok(_services.GetListFiles(request.prurl));
        }
    }

    public class PullRequestApiGetRequestBody
    {
        public string prurl { get; set; }
    }
}
