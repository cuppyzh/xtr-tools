using cuppyzh.xtrtools.poadocumentgenerator.Exceptions;
using cuppyzh.xtrtools.poadocumentgenerator.Services.Interfaces;
using cuppyzh.xtrtools.poadocumentgenerator.Utilities;

namespace cuppyzh.xtrtools.poadocumentgenerator.Services
{
    public class UserServices: IUserServices
    {
        private readonly ApiCallServices apiCallServices = new ApiCallServices();

        public bool IsAuthenticated()
        {
            var endpoint = UrlUtils.GetCredentialTestEndpoint();
            var response = apiCallServices.SendGetRequest(endpoint);

            if (response == null
                || response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new XtoolsException("API response to check credential return null. Please check your connection");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new XtoolsException("API response to check credential return Unauthorized. Please check your credential");
            }

            return true;
        }
    }
}
