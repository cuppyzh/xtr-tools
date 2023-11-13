namespace cuppyzh.xtrtools.poadocumentgenerator.Utilities
{
    public static class UrlUtils
    {
        public static string GetCredentialTestEndpoint()
        {
            return ApplicationSettings.Git.Endpoints.MainUrl + ApplicationSettings.Git.Endpoints.TestApiUrl;
        }

        public static string GetPrUrlPreview()
        {
            return ApplicationSettings.Git.Endpoints.MainUrl + ApplicationSettings.Git.Endpoints.PrUrlPreview;
        }
    }
}
