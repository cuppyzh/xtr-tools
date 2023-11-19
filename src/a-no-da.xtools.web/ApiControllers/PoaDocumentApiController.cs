using Microsoft.AspNetCore.Mvc;

namespace a_no_da.xtools.web.ApiControllers
{
    [ApiController]
    [Route("api/v1/poadocument")]
    public class PoaDocumentApiController: ControllerBase
    {
        public IActionResult Index()
        {
            return Ok("Index");
        }
    }
}
