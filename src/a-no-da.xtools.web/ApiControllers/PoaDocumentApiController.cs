using a_no_da.xtools.core;
using a_no_da.xtools.core.Attributes;
using a_no_da.xtools.modules.poadocument;
using a_no_da.xtools.modules.poadocument.Models.Requests;
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

        public PoaDocumentApiController(IDocumentServices documentServices, IPRChangesServices pRChangesServices)
        {
            _documentServices = documentServices;
            _pRChangesServices = pRChangesServices;
        }

        public IActionResult Index()
        {
            return Ok("Index");
        }

        [HttpPost("pr-changes/get")]
        public IActionResult Get([FromForm] GetPrChangesRequest request)
        {
            var result = _pRChangesServices.GetListFiles(request);
            return Ok(result);
        }

        [HttpPost("security-assessment/export")]
        public IActionResult Export([FromBody] ExportPrChangesRequest request)
        {
            var workbookStream = _documentServices.Export(request);
            string filename = $"{DateTime.Now.ToShortDateString()}-{request.ProjectRepository}-{request.PRId}.xlsx";

            Response.Headers["Content-Disposition"] = $"attachment;filename={filename}";
            return File(workbookStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }
    }
}
