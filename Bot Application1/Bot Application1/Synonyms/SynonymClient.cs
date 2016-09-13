using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bot_Application1.SynonymClient
{
    public class SynonymClient
    {
        private readonly string _url = "http://words.bighugelabs.com/api/2/9a197ef778c7f9098a3b71e986daf0cf/{0}/json";

        /// <summary>
        /// The default resolver.
        /// </summary>
        private static readonly CamelCasePropertyNamesContractResolver defaultResolver = new CamelCasePropertyNamesContractResolver();

        /// <summary>
        /// The settings
        /// </summary>
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = defaultResolver
        };

        /// <summary>
        /// The JSON content type header.
        /// </summary>
        private const string JsonContentTypeHeader = "application/json";

        public async Task<TResponse> GetSynonymAsync<TResponse>(string word)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(string.Format(_url, word));

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = await client.GetAsync(""); 
            if (response.IsSuccessStatusCode)
            {
                string responseContent = null;
                if (response.Content != null)
                {
                    responseContent = await response.Content.ReadAsStringAsync();
                }

                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    return JsonConvert.DeserializeObject<TResponse>(responseContent, settings);
                }

                return default(TResponse);
            }
            else
            {
                return default(TResponse);
                //if (response.Content != null && response.Content.Headers.ContentType.MediaType.Contains(JsonContentTypeHeader))
                //{
                //    var errorObjectString = await response.Content.ReadAsStringAsync();
                //    ClientError errorCollection = JsonConvert.DeserializeObject<ClientError>(errorObjectString);
                //    if (errorCollection != null)
                //    {
                //        throw new ClientException(errorCollection, response.StatusCode);
                //    }
                //}

                //response.EnsureSuccessStatusCode();
            }

            return default(TResponse);

        }
    }
}