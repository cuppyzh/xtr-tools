using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_no_da.xtools.modules.poadocument.Utilities
{
    public static class UrlUtils
    {
        public static string GetCredentialTestEndpoint()
        {
            return ModuleConfig.Config.Git.Endpoints.MainUrl + ModuleConfig.Config.Git.Endpoints.TestApiUrl;
        }

        public static string GetPrUrlPreview()
        {
            return ModuleConfig.Config.Git.Endpoints.MainUrl + ModuleConfig.Config.Git.Endpoints.PrUrlPreview;
        }

        public static string GetPrFileChanges()
        {
            return ModuleConfig.Config.Git.Endpoints.MainUrl + ModuleConfig.Config.Git.Endpoints.FileChangesUrl;
        }
    }
}
