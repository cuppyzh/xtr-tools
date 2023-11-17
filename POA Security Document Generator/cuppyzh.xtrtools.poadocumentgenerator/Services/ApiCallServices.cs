using Microsoft.Extensions.ObjectPool;
using System.Text;
using System;
using cuppyzh.xtrtools.poadocumentgenerator.Utilities;
using cuppyzh.xtrtools.poadocumentgenerator.Services.Interfaces;

namespace cuppyzh.xtrtools.poadocumentgenerator.Services
{
    public class ApiCallServices: IApiCallServices
    {
        public HttpResponse SendPostRequest(string endpoint, object requestBody)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage SendGetRequest(string endpoint)
        {
            try
            {
                HttpMessageHandler handler = new HttpClientHandler();

                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(endpoint),
                    Timeout = new TimeSpan(0, 2, 0)
                };

                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + _GetCredential());
                HttpResponseMessage response = httpClient.GetAsync(endpoint).Result;

                return response;
            } catch(Exception ex)
            {
                return null;
            }
        }

        private string _GetCredential()
        {
            //This is the key section you were missing    
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{ApplicationSettings.Git.Credential.Username}:{ApplicationSettings.Git.Credential.Password}");
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
