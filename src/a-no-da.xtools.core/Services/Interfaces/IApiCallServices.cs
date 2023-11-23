using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_no_da.xtools.core.Services.Interfaces
{
    public interface IApiCallServices
    {
        HttpResponse SendPostRequest(string endpoint, object requestBody);
        HttpResponseMessage SendGetRequest(string endpoint);
        HttpResponseMessage SendGetRequest(string endpoint, string credentials);
        string GetStashCredentials();
    }
}
