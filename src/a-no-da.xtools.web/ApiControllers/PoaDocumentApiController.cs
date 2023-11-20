using a_no_da.xtools.core;
using a_no_da.xtools.core.Attributes;
using a_no_da.xtools.modules.poadocument.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace a_no_da.xtools.web.ApiControllers
{
    [ApiController]
    [Route("api/v1/poadocument")]
    [XToolsExceptionAttribute]
    public class PoaDocumentApiController : ControllerBase
    {
        private readonly IDocumentServices _documentServices;
        private readonly IPRChangesServices _pRChangesServices;
        private readonly CoreConfig _configs;

        public PoaDocumentApiController(IDocumentServices documentServices, IPRChangesServices pRChangesServices, IOptions<CoreConfig> configs)
        {
            _documentServices = documentServices;
            _pRChangesServices = pRChangesServices;
            _configs = configs.Value;
        }

        public IActionResult Index()
        {
            return Ok("Index");
        }

        [HttpPost("pr-changes/get")]
        public IActionResult Get([FromForm] object request)
        {
            throw new NotImplementedException();
        }

        [HttpPost("security-assessment/export")]
        public IActionResult Export([FromBody] object request)
        {
            throw new NotImplementedException();
        }
    }
}
