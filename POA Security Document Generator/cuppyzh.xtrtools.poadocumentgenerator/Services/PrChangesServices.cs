using ClosedXML.Excel;
using cuppyzh.xtrtools.poadocumentgenerator.Controllers;
using cuppyzh.xtrtools.poadocumentgenerator.Utilities;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace cuppyzh.xtrtools.poadocumentgenerator.Services
{
    public class PrChangesServices
    {
        private readonly ApiCallServices _callServices = new ApiCallServices();

        public object GetListFiles(string prurl)
        {
            List<object> result = new List<object>();

            var endpoint = _GeneratePrChangesUrl(prurl);
            var response = _callServices.SendGetRequest(endpoint);

            string responseBody = response.Content.ReadAsStringAsync().Result;
            JsonDocument doc = JsonDocument.Parse(responseBody);

            JsonElement root = doc.RootElement;
            JsonElement values = root.GetProperty("values");

            foreach (var jsonElement in values.EnumerateArray())
            {
                result.Add(new
                {
                    Path = jsonElement.GetProperty("path").GetProperty("toString").GetString(),
                    Type = jsonElement.GetProperty("type").GetString()
                });
            }

            return new
            {
                ProjectName = _GetProjectName(prurl),
                ProjectRepository = _GetRepoName(prurl),
                PrId = _GetPrId(prurl),
                CommitId = root.GetProperty("fromHash"),
                SinceCommitId = root.GetProperty("toHash"),
                Changes = result
            };
        }

        public MemoryStream Export(ExportApiRequestBody request)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Exported Sheet");
                worksheet.Cell("A1").Value = "Hello World!";
                worksheet.Cell("A2").FormulaA1 = "MID(A1, 7, 5)";
                workbook.SaveAs("HelloWorld.xlsx");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return memoryStream;
                };
            };
        }

        private string _GeneratePrChangesUrl(string prurl)
        {
            var url = ApplicationSettings.Git.Endpoints.MainUrl
                + ApplicationSettings.Git.Endpoints.PrChangesUrl;
            url = url.Replace("{projectName}", _GetProjectName(prurl));
            url = url.Replace("{projectRepository}", _GetRepoName(prurl));
            url = url.Replace("{prId}", _GetPrId(prurl));

            return url;
        }

        private string _GetProjectName(string prUrl)
        {
            Regex regex = new Regex("(?<=\\/projects\\/)(.*)(?=\\/repos)");
            var match = regex.Match(prUrl);
            if (!match.Success)
            {
                throw new Exception("Project Name is not found");
            }

            return match.Value;
        }

        private string _GetRepoName(string prUrl)
        {
            Regex regex = new Regex("(?<=\\/repos\\/)(.*)(?=\\/pull-requests)");
            var match = regex.Match(prUrl);
            if (!match.Success)
            {
                throw new Exception("Repository Name is not found");
            }

            return match.Value;
        }

        private string _GetPrId(string prUrl)
        {
            Regex regex = new Regex("(?<=\\/pull-requests\\/)(.*)(?=\\/overview)");
            var match = regex.Match(prUrl);
            if (!match.Success)
            {
                throw new Exception("Repository Name is not found");
            }

            return match.Value;
        }
    }
}
