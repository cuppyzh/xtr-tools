using a_no_da.xtools.core.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_no_da.xtools.core.Services
{
    public class ApiCallServices: IApiCallServices
    {
        private readonly ILogger<ApiCallServices> _logger;
        private readonly CoreConfig _coreConfig;

        public ApiCallServices(ILogger<ApiCallServices> logger, IOptions<CoreConfig> coreConfig)
        {
            _logger = logger;
            _coreConfig = coreConfig.Value;
        }

        public HttpResponseMessage SendGetRequest(string endpoint, string credentials)
        {
            try
            {
                _logger.LogInformation($"Endpoint: {endpoint}");

                HttpMessageHandler handler = new HttpClientHandler();

                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(endpoint),
                    Timeout = new TimeSpan(0, 2, 0)
                };

                if (!string.IsNullOrEmpty(credentials))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);
                }

                HttpResponseMessage response = httpClient.GetAsync(endpoint).Result;

                _logger.LogInformation($"Status Response: {response.StatusCode}");

                return response;
            }
            catch
            {
                return null;
            }
        }

        public string GetStashCredentials()
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{_coreConfig.Git.Credential.Username}:{_coreConfig.Git.Credential.Password}");
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
