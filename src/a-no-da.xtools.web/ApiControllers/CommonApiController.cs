using Microsoft.AspNetCore.Mvc;

namespace a_no_da.xtools.web.ApiControllers
{
    [ApiController]
    [Route("api/v1/common")]
    public class CommonApiController: ControllerBase
    {
        [HttpGet("healthcheck")]
        public IActionResult HealthCheckApi()
        {
            return Ok();
        }
    }
}
