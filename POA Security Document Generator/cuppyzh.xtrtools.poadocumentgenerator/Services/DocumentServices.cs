using ClosedXML.Excel;
using cuppyzh.xtrtools.poadocumentgenerator.Controllers;

namespace cuppyzh.xtrtools.poadocumentgenerator.Services
{
    public class DocumentServices
    {
        public MemoryStream Export(ExportApiRequestBody request)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Exported Sheet");

                worksheet.Cell("A1").Value = "Filename";
                worksheet.Cell("A1").Style.Font.Bold = true;
                worksheet.Cell("B1").Value = "Description";
                worksheet.Cell("B1").Style.Font.Bold = true;
                worksheet.Cell("C1").Value = "Line of codes";
                worksheet.Cell("C1").Style.Font.Bold = true;
                worksheet.Range("C1:D1").Row(1).Merge();
                worksheet.Cell("E1").Value = "Changes";
                worksheet.Cell("E1").Style.Font.Bold = true;

                for(int i=0; i<request.Files.Count(); i++)
                {
                    worksheet.Cell($"A{i + 2}").Value = request.Files[i];
                }

                workbook.SaveAs("HelloWorld.xlsx");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return memoryStream;
                };
            };
        }
    }
}
