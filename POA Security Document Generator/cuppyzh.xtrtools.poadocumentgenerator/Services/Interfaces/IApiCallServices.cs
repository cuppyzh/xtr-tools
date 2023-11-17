namespace cuppyzh.xtrtools.poadocumentgenerator.Services.Interfaces
{
    public interface IApiCallServices
    {
        HttpResponse SendPostRequest(string endpoint, object requestBody);
        HttpResponseMessage SendGetRequest(string endpoint);
    }
}
