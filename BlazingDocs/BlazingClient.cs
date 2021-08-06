using BlazingDocs.Exceptions;
using BlazingDocs.Models;
using BlazingDocs.Parameters;
using BlazingDocs.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazingDocs
{
    public interface IBlazingClient
    {
        /// <summary>
        /// Gets account info.
        /// </summary>
        Task<AccountModel> GetAccountAsync();

        /// <summary>
        /// Gets templates list.
        /// </summary>
        Task<List<TemplateModel>> GetTemplatesAsync(string path = null);

        /// <summary>
        /// Gets usage info.
        /// </summary>
        Task<UsageModel> GetUsageAsync();

        /// <summary>
        /// Executes merge operation with template id.
        /// </summary>
        Task<OperationModel> MergeAsync(string data, string filename, MergeParameters parameters, Guid template);

        /// <summary>
        /// Executes merge operation with template form file.
        /// </summary>
        Task<OperationModel> MergeAsync(string data, string filename, MergeParameters parameters, FormFile template);

        /// <summary>
        /// Executes merge operation with template path.
        /// </summary>
        Task<OperationModel> MergeAsync(string data, string filename, MergeParameters parameters, string template);
    }

    /// <summary>
    /// BlazingDocs API client.
    /// </summary>
    public class BlazingClient : IBlazingClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly string _apiKey;

        private readonly string _baseUrl = "https://api.blazingdocs.com";

        /// <summary>
        /// Serialized object to json.
        /// </summary>
        private string Serialize<T>(T source)
        {
            return JsonSerializer.Serialize(source, _jsonOptions);
        }

        /// <summary>
        /// Executes merge operation with generic template.
        /// </summary>
        private async Task<OperationModel> MergeAsyncInner(string data, string filename, MergeParameters parameters, object template)
        {
            var content = new MultipartFormDataContent();

            if (string.IsNullOrEmpty(data)) // check data provided
            {
                throw new ArgumentException("Data is not provided", nameof(data));
            }

            content.Add(new StringContent(data, Encoding.UTF8), "Data");

            if (string.IsNullOrEmpty(filename)) // check output filename provided
            {
                throw new ArgumentException("Output filename is not provided", nameof(data));
            }

            content.Add(new StringContent(filename, Encoding.UTF8), "OutputName");

            if (parameters == null) // check merge parameters provided
            {
                throw new ArgumentException("Merge parameters are not provided", nameof(parameters));
            }

            content.Add(new StringContent(Serialize(parameters), Encoding.UTF8, "application/json"), "MergeParameters");

            if (template == null) // check template porvided
            {
                throw new ArgumentException("Template is not provided", nameof(template));
            }

            if (template is Guid guid) // check template parameter is guid
            {
                content.Add(new StringContent(guid.ToString(), Encoding.UTF8), "Template");
            }
            else if (template is string relativePath) // check template parameter is relative path
            {
                content.Add(new StringContent(relativePath.Replace('\\', '/').Trim(), Encoding.UTF8), "Template");
            }
            else if (template is FormFile file) // check template parameter is file
            {
                content.Add(new StreamContent(file.Content), "Template", file.Name);
            }

            var endpoint = new Uri($"{_baseUrl}/operation/merge");

            HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);

            string raw = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK) // check response status
            {
                throw new BlazingException(response.StatusCode, Deserialize<ErrorModel>(raw).Message);
            }

            return Deserialize<OperationModel>(raw);
        }

        /// <summary>
        /// Deserealizes json to object.
        /// </summary>
        private T Deserialize<T>(string source)
        {
            return JsonSerializer.Deserialize<T>(source, _jsonOptions);
        }

        /// <summary>
        /// Gets account info.
        /// </summary>
        public async Task<AccountModel> GetAccountAsync()
        {
            var endpoint = new Uri($"{_baseUrl}/account");

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            string raw = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK) // check response status
            {
                throw new BlazingException(response.StatusCode, Deserialize<ErrorModel>(raw).Message);
            }

            return Deserialize<AccountModel>(raw);
        }

        /// <summary>
        /// Gets templates list.
        /// </summary>
        public async Task<List<TemplateModel>> GetTemplatesAsync(string path = null)
        {
            var endpoint = new Uri($"{_baseUrl}/templates/{path}");

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            string raw = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK) // check response status
            {
                throw new BlazingException(response.StatusCode, Deserialize<ErrorModel>(raw).Message);
            }

            return Deserialize<List<TemplateModel>>(raw);
        }

        /// <summary>
        /// Gets usage info.
        /// </summary>
        public async Task<UsageModel> GetUsageAsync()
        {
            var endpoint = new Uri($"{_baseUrl}/usage");

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            string raw = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK) // check response status
            {
                throw new BlazingException(response.StatusCode, Deserialize<ErrorModel>(raw).Message);
            }

            return Deserialize<UsageModel>(raw);
        }

        /// <summary>
        /// Executes merge operation with template id.
        /// </summary>
        public async Task<OperationModel> MergeAsync(string data, string filename, MergeParameters parameters, Guid template)
        {
            return await MergeAsyncInner(data, filename, parameters, template);
        }

        /// <summary>
        /// Executes merge operation with template form file.
        /// </summary>
        public async Task<OperationModel> MergeAsync(string data, string filename, MergeParameters parameters, FormFile template)
        {
            return await MergeAsyncInner(data, filename, parameters, template);
        }

        /// <summary>
        /// Executes merge operation with template path.
        /// </summary>
        public async Task<OperationModel> MergeAsync(string data, string filename, MergeParameters parameters, string template)
        {
            return await MergeAsyncInner(data, filename, parameters, template);
        }

        /// <summary>
        /// Creates new instance of client.
        /// </summary>
        public BlazingClient(string apiKey)
        {
            _httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(10)
            };

            _httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);

            _jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            _jsonOptions.Converters.Add(new JsonStringEnumConverter());

            _apiKey = apiKey;
        }
    }
}
