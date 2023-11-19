namespace cuppyzh.xtrtools.poadocumentgenerator.Models
{
    public class PrFileChangesResponseModel
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
