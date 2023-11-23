using a_no_da.xtools.modules.poadocument.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_no_da.xtools.modules.poadocument.Services.Interfaces
{
    public interface IDocumentServices
    {
        MemoryStream Export(ExportPrChangesRequest request);
    }
}
