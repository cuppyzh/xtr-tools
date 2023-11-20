using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_no_da.xtools.core
{
    public class CoreConfig
    {
        public const string CONFIG_PATH = "Configs/GitConfig.json";
        public const string SECTION_NAME = "CoreConfig";
        public GitConfigModel Git { get; set; } = new GitConfigModel();
    }

    public class GitConfigModel
    {
        public GitCredentialConfigModel Credential { get; set; }
    }

    public class GitCredentialConfigModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
