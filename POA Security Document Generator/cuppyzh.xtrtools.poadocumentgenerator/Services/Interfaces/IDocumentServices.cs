using cuppyzh.xtrtools.poadocumentgenerator.Controllers;

namespace cuppyzh.xtrtools.poadocumentgenerator.Services.Interfaces
{
    public interface IDocumentServices
    {
        MemoryStream Export(ExportApiRequestBody request);
    }
}
