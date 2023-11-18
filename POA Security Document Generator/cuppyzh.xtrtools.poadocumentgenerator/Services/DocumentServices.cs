using ClosedXML.Excel;
using cuppyzh.xtrtools.poadocumentgenerator.Controllers;
using cuppyzh.xtrtools.poadocumentgenerator.Models;
using cuppyzh.xtrtools.poadocumentgenerator.Services.Interfaces;
using cuppyzh.xtrtools.poadocumentgenerator.Utilities;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ExceptionServices;
using System.Text.Json.Serialization;

namespace cuppyzh.xtrtools.poadocumentgenerator.Services
{
    public class DocumentServices : IDocumentServices
    {
        private readonly IApiCallServices _apiCallService;
        private readonly ILogger<DocumentServices> _logger;

        public DocumentServices(IApiCallServices apiCallServices, ILogger<DocumentServices> logger)
        {
            _apiCallService = apiCallServices;
            _logger = logger;
        }

        public MemoryStream Export(ExportApiRequestBody request)
        {
            _logger.LogInformation($"Processing request: {request.ProjectName}/{request.ProjectRepository}/{request.PRId}");
            _logger.LogInformation($"Total Files: {request.Files.Count}");

            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Exported Sheet");

                worksheet.Cell("A1").Value = "Filename";
                worksheet.Cell("A1").Style.Font.Bold = true;
                worksheet.Cell("B1").Value = "Context";
                worksheet.Cell("B1").Style.Font.Bold = true;
                worksheet.Cell("C1").Value = "Description";
                worksheet.Cell("C1").Style.Font.Bold = true;
                worksheet.Cell("D1").Value = "Line of codes";
                worksheet.Cell("D1").Style.Font.Bold = true;
                worksheet.Range("D1:E1").Row(1).Merge();
                worksheet.Cell("G1").Value = "Changes";
                worksheet.Cell("G1").Style.Font.Bold = true;

                int currentRowIndex = 2;

                for (int i = 0; i < request.Files.Count(); i++)
                {
                    worksheet.Cell($"A{currentRowIndex}").Value = request.Files[i].File;
                    worksheet.Cell($"B{currentRowIndex}").Value = request.Files[i].Context;

                    var endpoint = UrlUtils.GetPrFileChanges()
                        .Replace("{projectName}", request.ProjectName)
                        .Replace("{projectRepository}", request.ProjectRepository)
                        .Replace("{commitId}", request.CommitId)
                        .Replace("{startCommitId}", request.SinceCommitId)
                        .Replace("{filePath}", request.Files[i].File)
                        .ToString();

                    var response = _apiCallService.SendGetRequest(endpoint);
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    var fileChanges = JsonConvert.DeserializeObject<PrFileChangesResponseModel>(responseBody);

                    var startRowIndex = currentRowIndex;
                    currentRowIndex = _InsertFileChanges(worksheet, fileChanges, currentRowIndex);
                    worksheet.Range($"A{startRowIndex}:A{currentRowIndex - 1}").Column(1).Merge();
                    worksheet.Range($"B{startRowIndex}:B{currentRowIndex - 1}").Column(1).Merge();
                    worksheet.Range($"C{startRowIndex}:C{currentRowIndex - 1}").Column(1).Merge();
                    currentRowIndex++;
                }

                worksheet.Columns().AdjustToContents();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return memoryStream;
                };
            };
        }

        private int _InsertFileChanges(IXLWorksheet worksheet, PrFileChangesResponseModel changes, int currentRowIndex)
        {
            for (int i = 0; i < changes.diffs[0].hunks[0].segments.Count; i++)
            {
                var segment = changes.diffs[0].hunks[0].segments[i];

                if (segment.type.Equals("CONTEXT", StringComparison.InvariantCultureIgnoreCase))
                {
                    SegmentType segmentType = SegmentType.Middle;
                    if (i == 0)
                    {
                        segmentType = SegmentType.First;
                    }
                    else if (i == changes.diffs[0].hunks[0].segments.Count - 1)
                    {
                        segmentType = SegmentType.Last;
                    }

                    currentRowIndex = _AddContextData(worksheet, segment, currentRowIndex, segmentType);
                    continue;
                }

                if (segment.type.Equals("ADDED", StringComparison.InvariantCultureIgnoreCase))
                {
                    currentRowIndex = _AddAddedData(worksheet, segment, currentRowIndex);
                    continue;
                }

                if (segment.type.Equals("REMOVED", StringComparison.InvariantCultureIgnoreCase))
                {
                    currentRowIndex = _AddRemovedData(worksheet, segment, currentRowIndex);
                    continue;
                }
            }

            return currentRowIndex;
        }

        private int _AddContextData(IXLWorksheet worksheet, SegmentModel segment, int currentRowIndex, SegmentType segmentType)
        {
            var processedLines = new List<LinesDataModel>();
            if (segment.lines.Count < ApplicationSettings.Document.MinDocumentContext)
            {
                processedLines = segment.lines;
            }
            else
            {
                if (segmentType == SegmentType.First)
                {
                    processedLines.AddRange(segment.lines.TakeLast(ApplicationSettings.Document.MinDocumentContext).ToList());
                }
                else if (segmentType == SegmentType.Last)
                {
                    processedLines.AddRange(segment.lines.Take(ApplicationSettings.Document.MinDocumentContext).ToList());
                }
                else
                {
                    processedLines.AddRange(segment.lines.Take(5).ToList());
                    processedLines.Add(new LinesDataModel()
                    {
                        source = null,
                        destination = null,
                        line = "..."
                    });
                    processedLines.Add(new LinesDataModel()
                    {
                        source = null,
                        destination = null,
                        line = "..."
                    });
                    processedLines.AddRange(segment.lines.TakeLast(5).ToList());
                }
            }

            foreach (var line in processedLines)
            {
                worksheet.Cell($"D{currentRowIndex}").Value = line.source;
                worksheet.Cell($"E{currentRowIndex}").Value = line.destination;
                worksheet.Cell($"G{currentRowIndex}").Value = $"'{line.line}";
                currentRowIndex++;
            }

            return currentRowIndex;
        }

        private int _AddAddedData(IXLWorksheet worksheet, SegmentModel segment, int currentRowIndex)
        {
            foreach (var line in segment.lines)
            {
                worksheet.Cell($"E{currentRowIndex}").Value = line.destination;
                worksheet.Cell($"F{currentRowIndex}").Value = "+";
                worksheet.Cell($"G{currentRowIndex}").Value = $"'{line.line}";

                worksheet.Cell($"D{currentRowIndex}").Style.Fill.BackgroundColor = XLColor.Lime;
                worksheet.Cell($"E{currentRowIndex}").Style.Fill.BackgroundColor = XLColor.Lime;
                worksheet.Cell($"F{currentRowIndex}").Style.Fill.BackgroundColor = XLColor.Lime;
                worksheet.Cell($"G{currentRowIndex}").Style.Fill.BackgroundColor = XLColor.Lime;

                currentRowIndex++;
            }

            return currentRowIndex;
        }

        private int _AddRemovedData(IXLWorksheet worksheet, SegmentModel segment, int currentRowIndex)
        {
            foreach (var line in segment.lines)
            {
                worksheet.Cell($"D{currentRowIndex}").Value = line.source;
                worksheet.Cell($"F{currentRowIndex}").Value = "-";
                worksheet.Cell($"G{currentRowIndex}").Value = $"'{line.line}";

                worksheet.Cell($"D{currentRowIndex}").Style.Fill.BackgroundColor = XLColor.Orange;
                worksheet.Cell($"F{currentRowIndex}").Style.Fill.BackgroundColor = XLColor.Orange;
                worksheet.Cell($"G{currentRowIndex}").Style.Fill.BackgroundColor = XLColor.Orange;
                worksheet.Cell($"H{currentRowIndex}").Style.Fill.BackgroundColor = XLColor.Orange;

                currentRowIndex++;
            }

            return currentRowIndex;
        }
    }
}
