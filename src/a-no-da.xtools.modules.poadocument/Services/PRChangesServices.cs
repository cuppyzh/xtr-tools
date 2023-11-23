using a_no_da.xtools.core.Exceptions;
using a_no_da.xtools.core.Services.Interfaces;
using a_no_da.xtools.modules.poadocument.Models.Requests;
using a_no_da.xtools.modules.poadocument.Models.Responses;
using a_no_da.xtools.modules.poadocument.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace a_no_da.xtools.modules.poadocument.Services
{
    public class PRChangesServices : IPRChangesServices
    {
        private readonly IApiCallServices _apiCallServices;

        public PRChangesServices(IApiCallServices apiCallServices)
        {
            _apiCallServices = apiCallServices;
        }

        public PRChangesResponse GetListFiles(GetPrChangesRequest request)
        {
            if (request == null)
                throw new XToolsValidationException("Request Body is null");

            if (string.IsNullOrEmpty(request.PRUrl))
                throw new BadHttpRequestException($"PRURL value is request is empty");

            List<FilesChangesResponse> files = new List<FilesChangesResponse>();

            var endpoint = _GeneratePrChangesUrl(request.PRUrl);
            var response = _apiCallServices.SendGetRequest(endpoint, _apiCallServices.GetStashCredentials());

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new XToolsException($"Request to {request.PRUrl} is not failed. Http Status Code: {response.StatusCode}");
            }

            string responseBody = response.Content.ReadAsStringAsync().Result;

            JsonDocument doc = JsonDocument.Parse(responseBody);

            if (doc == null)
            {
                throw new XToolsException($"Response from {request.PRUrl} is failed to be parsed. Response Body: {responseBody}");
            }

            JsonElement root = doc.RootElement;
            JsonElement values = root.GetProperty("values");

            foreach (var jsonElement in values.EnumerateArray())
            {
                files.Add(new FilesChangesResponse
                {
                    Path = jsonElement.GetProperty("path").GetProperty("toString").GetString(),
                    Type = jsonElement.GetProperty("type").GetString()
                });
            }

            return new PRChangesResponse()
            {
                ProjectName = _GetProjectName(request.PRUrl),
                ProjectRepository = _GetRepoName(request.PRUrl),
                PrId = _GetPrId(request.PRUrl),
                CommitId = root.GetProperty("toHash").ToString(),
                SinceCommitId = root.GetProperty("fromHash").ToString(),
                Changes = files
            };
        }
        private string _GeneratePrChangesUrl(string prurl)
        {
            var url = ModuleConfig.Config.Git.Endpoints.MainUrl
                + ModuleConfig.Config.Git.Endpoints.PrChangesUrl;
            url = url.Replace("{projectName}", _GetProjectName(prurl));
            url = url.Replace("{projectRepository}", _GetRepoName(prurl));
            url = url.Replace("{prId}", _GetPrId(prurl));

            return url;
        }

        private string _GetProjectName(string prUrl)
        {
            Regex regex = new Regex("(?<=\\/projects\\/)(.*)(?=\\/repos)");
            var match = regex.Match(prUrl);
            if (!match.Success)
            {
                throw new XToolsException("Project Name is not found");
            }

            return match.Value;
        }

        private string _GetRepoName(string prUrl)
        {
            Regex regex = new Regex("(?<=\\/repos\\/)(.*)(?=\\/pull-requests)");
            var match = regex.Match(prUrl);
            if (!match.Success)
            {
                throw new XToolsException("Repository Name is not found");
            }

            return match.Value;
        }

        private string _GetPrId(string prUrl)
        {
            Regex regex = new Regex("(?<=\\/pull-requests\\/)(.*)(?=\\/overview)");
            var match = regex.Match(prUrl);
            if (!match.Success)
            {
                throw new XToolsException("Repository Name is not found");
            }

            return match.Value;
        }
    }
}
