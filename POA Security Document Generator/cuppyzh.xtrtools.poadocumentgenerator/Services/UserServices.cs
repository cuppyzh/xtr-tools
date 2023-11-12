using cuppyzh.xtrtools.poadocumentgenerator.Utilities;

namespace cuppyzh.xtrtools.poadocumentgenerator.Services
{
    public class UserServices
    {
        private readonly ApiCallServices apiCallServices = new ApiCallServices();

        public bool IsAuthenticated()
        {
            var endpoint = UrlUtils.GetCredentialTestEndpoint();
            var response = apiCallServices.SendGetRequest(endpoint);

            if (response == null)
            {
                throw new Exception("");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return false;
            }

            return true;
        }
    }
}
