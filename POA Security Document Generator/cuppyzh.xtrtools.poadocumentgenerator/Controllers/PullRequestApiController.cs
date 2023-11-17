using ClosedXML.Excel;
using cuppyzh.xtrtools.poadocumentgenerator.Exceptions;
using cuppyzh.xtrtools.poadocumentgenerator.Services;
using cuppyzh.xtrtools.poadocumentgenerator.Services.Interfaces;
using cuppyzh.xtrtools.poadocumentgenerator.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace cuppyzh.xtrtools.poadocumentgenerator.Controllers
{
    [ApiController]
    [Route("api/v1/pr-changes")]
    public class PullRequestApiController : ControllerBase
    {
        private readonly IPrChangesServices _prChangesServices;
        private readonly IDocumentServices _documentServices;
        private readonly ILogger<PullRequestApiController> _logger;

        public PullRequestApiController(ILogger<PullRequestApiController> logger, IDocumentServices documentServices, IPrChangesServices prChangesServices)
        {
            _logger = logger;
            _documentServices = documentServices;
            _prChangesServices = prChangesServices;
        }

        [HttpPost("get")]
        public IActionResult Get([FromForm] PullRequestApiGetRequestBody request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(request.prurl))
            {
                return BadRequest();
            }

            _logger.LogDebug($"Body Request: {JsonConvert.SerializeObject(request)}");

            try
            {
                var result = _prChangesServices.GetListFiles(request.prurl);

                return Ok(result);
            }
            catch (XtoolsException ex)
            {
                _logger.LogException(ex);
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return StatusCode(500, "500 - Internal Server Error");
            }
        }

        [HttpPost("export")]
        public IActionResult Export([FromBody] ExportApiRequestBody request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            if (!request.IsValid())
            {
                return BadRequest();
            }

            _logger.LogDebug($"Body Request: {JsonConvert.SerializeObject(request)}");

            try
            {
                var workbook = _documentServices.Export(request);
                string filename = $"{request.ProjectRepository}-PR-{request.PRId}.xlsx";

                Response.Headers["Content-Disposition"] = $"attachment;filename={filename}";
                return File(workbook.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            catch (XtoolsException ex)
            {
                _logger.LogException(ex);
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return StatusCode(500, "500 - Internal Server Error");
            }
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

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(ProjectName))
            {
                return false;
            }

            if (string.IsNullOrEmpty(ProjectRepository))
            {
                return false;
            }

            if (string.IsNullOrEmpty(PRId))
            {
                return false;
            }

            if (string.IsNullOrEmpty(CommitId))
            {
                return false;
            }

            if (string.IsNullOrEmpty(SinceCommitId))
            {
                return false;
            }

            if (Files == null || Files.Count == 0)
            {
                return false;
            }

            return true;
        }
    }

    public class FilesRequest
    {
        public string File { get; set; }
        public string Context { get; set; }
    }
}
