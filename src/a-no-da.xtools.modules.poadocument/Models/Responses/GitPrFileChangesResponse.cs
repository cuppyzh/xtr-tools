using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_no_da.xtools.modules.poadocument.Models.Responses
{
    public class GitPrFileChangesResponse
    {
        public List<DiffsModel> diffs { get; set; }
    }

    public class DiffsModel
    {
        public List<HunksModel> hunks { get; set; }
    }

    public class HunksModel
    {
        public List<SegmentModel> segments { get; set; }
    }

    public class SegmentModel
    {
        public string type { get; set; }
        public List<LinesDataModel> lines { get; set; }
    }

    public class LinesDataModel
    {
        public int? destination { get; set; }
        public int? source { get; set; }
        public string line { get; set; }
    }

    public enum SegmentType
    {
        First,
        Middle,
        Last
    }
}
