using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_no_da.xtools.modules.poadocument
{
    public static class ModuleConfig
    {
        public const string CONFIG_PATH = "Configs/PoaDocumentConfig.json";
        public const string SECTION_NAME = "PoaDocumentConfig";

        public static ConfigModel Config { get; set; } = new ConfigModel();

        public class ConfigModel
        {
            public GitConfigModel Git { get; set; }

            public DocumentConfigModel Document { get; set; }
        }

        public class GitConfigModel
        {
            public GitEndpointConfigModel Endpoints { get; set; }
        }

        public class DocumentConfigModel
        {
            public int MinDocumentContext { get; set; }
            public string CodeAddedGreenArgb { get; set; }
            public string CodeRemovedRedArgb { get; set; }

            public XLColor GetCodeAddedColor()
            {
                if (string.IsNullOrEmpty(CodeAddedGreenArgb))
                {
                    return XLColor.Lime;
                }

                if (CodeAddedGreenArgb.Split(',').Length != 3)
                {
                    return XLColor.Lime;
                }

                int[] rgb = CodeAddedGreenArgb.Split(',').Select(x => int.Parse(x)).ToArray();
                return XLColor.FromArgb(rgb[0], rgb[1], rgb[2]);
            }

            public XLColor GetCodeRemovedColor()
            {
                if (string.IsNullOrEmpty(CodeRemovedRedArgb))
                {
                    return XLColor.Orange;
                }

                if (CodeRemovedRedArgb.Split(',').Length != 3)
                {
                    return XLColor.Orange;
                }

                int[] rgb = CodeRemovedRedArgb.Split(',').Select(x => int.Parse(x)).ToArray();
                return XLColor.FromArgb(rgb[0], rgb[1], rgb[2]);
            }
        }

        public class GitEndpointConfigModel
        {
            public string MainUrl { get; set; }
            public string PrUrlPreview { get; set; }
            public string TestApiUrl { get; set; }
            public string PrChangesUrl { get; set; }
            public string FileChangesUrl { get; set; }
        }
    }
}
