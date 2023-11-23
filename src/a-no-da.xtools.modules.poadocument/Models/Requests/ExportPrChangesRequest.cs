using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_no_da.xtools.modules.poadocument.Models.Requests
{
    public class ExportPrChangesRequest
    {
        public string ProjectName { get; set; }
        public string ProjectRepository { get; set; }
        public string PRId { get; set; }
        public string CommitId { get; set; }
        public string SinceCommitId { get; set; }
        public List<FileRequest> Files { get; set; }

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

        public class FileRequest
        {
            public string File { get; set; }
            public string Context { get; set; }
        }
    }
}
