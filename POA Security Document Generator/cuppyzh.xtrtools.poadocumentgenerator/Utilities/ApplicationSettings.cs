using System.Net;

namespace cuppyzh.xtrtools.poadocumentgenerator.Utilities
{
    public static class ApplicationSettings
    {
        public const string GIT_SECTION_NAME = "Git";
        public const string DOCUMENT_SECTION_NAME = "Document";

        public static GitConfigModel Git { get; set; } = new GitConfigModel();
        public static DocumentConfigModel Document { get; set; } = new DocumentConfigModel();
    }

    public class GitConfigModel
    {
        public GitCredentialConfigModel Credential { get; set; }
        public GitEndpointConfigModel Endpoints { get; set; }
    }

    public class DocumentConfigModel
    {
        public int MinDocumentContext { get; set; }
    }

    public class GitCredentialConfigModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
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
