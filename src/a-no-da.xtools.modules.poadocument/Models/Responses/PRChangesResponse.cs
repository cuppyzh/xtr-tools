using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_no_da.xtools.modules.poadocument.Models.Responses
{
    public class PRChangesResponse
    {
        public string ProjectName { get; set; }
        public string ProjectRepository { get; set; }
        public string PrId { get; set; }
        public string CommitId { get; set; }
        public string SinceCommitId { get; set; }
        public List<FilesChangesResponse> Changes { get; set; }
    }

    public class FilesChangesResponse
    {
        public string Path { get; set; }
        public string Type { get; set; }    
    }
}
