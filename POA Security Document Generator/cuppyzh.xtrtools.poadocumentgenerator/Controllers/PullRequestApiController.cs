using ClosedXML.Excel;
using cuppyzh.xtrtools.poadocumentgenerator.Services;
using Microsoft.AspNetCore.Mvc;

namespace cuppyzh.xtrtools.poadocumentgenerator.Controllers
{
    [ApiController]
    [Route("api/v1/pr-changes")]
    public class PullRequestApiController : ControllerBase
    {
        private readonly PrChangesServices _services = new PrChangesServices();
        private readonly DocumentServices _documentServices = new DocumentServices();

        [HttpPost("get")]
        public IActionResult Get([FromForm] PullRequestApiGetRequestBody request)
        {

            return Ok(_services.GetListFiles(request.prurl));
        }

        [HttpPost("export")]
        public IActionResult Export([FromBody] ExportApiRequestBody request)
        {
            var workbook = _documentServices.Export(request);
            string filename = $"{request.ProjectRepository}-PR-{request.PRId}.xlsx";

            Response.Headers["Content-Disposition"] = $"attachment;filename={filename}";
            return File(workbook.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }

        private Stream GetStream(XLWorkbook excelWorkbook)
        {
            Stream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }
    }

    public class PullRequestApiGetRequestBody
    {
        public string prurl { get; set; }
    }

    public class ExportApiRequestBody
    {
        public string ProjectName { get; set; }
        public string ProjectRepository { get; set; }
        public string PRId { get; set; }
        public string CommitId { get; set; }
        public string SinceCommitId { get; set; }
        public List<FilesRequest> Files { get; set; }
    }

    public class FilesRequest
    {
        public string File { get; set; }
        public string Context { get; set; }
    }
}
