using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bot_Application1
{
    public class PartOfSpeachTaggingService
    {
        private string _address = "https://api.projectoxford.ai/linguistics/v1.0/analyze";
        private string _key1 = "f3f535f1fc8b495b97cf599d94f9e8e0";
        private string _key2 = "019eb777f1a649de83d1d47bd43582a6";

        /// <summary>
        /// Initializes a new instance of <see cref="LinguisticsClient"/> class.
        /// </summary>
        private static readonly LinguisticsClient Client = new LinguisticsClient("f3f535f1fc8b495b97cf599d94f9e8e0");

        /// <summary>
        /// Default jsonserializer settings
        /// </summary>
        private static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        // List analyzers
        Analyzer[] supportedAnalyzers = null;

        public  PartOfSpeachTaggingService()
        {
            
        }

        public async Task Init()
        {
            try
            {
                supportedAnalyzers = await ListAnalyzers();
                var analyzersAsJson = JsonConvert.SerializeObject(supportedAnalyzers, Formatting.Indented, jsonSerializerSettings);
                Console.WriteLine("Supported analyzers: " + analyzersAsJson);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Failed to list supported analyzers: " + e.ToString());
                Environment.Exit(1);
            }
        }

        public async Task<AnalyzeTextResult[]> Analyze(string text)
        {
            // Analyze text with all available analyzers
            var analyzeTextRequest = new AnalyzeTextRequest()
            {
                Language = "en",
                AnalyzerIds = supportedAnalyzers.Select(analyzer => analyzer.Id).ToArray(),
                Text = text
            };

            try
            {
                var analyzeTextResults = await AnalyzeText(analyzeTextRequest);
                //var resultsAsJson = JsonConvert.SerializeObject(analyzeTextResults, Formatting.Indented, jsonSerializerSettings);
                return analyzeTextResults;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// List analyzers synchronously.
        /// </summary>
        /// <returns>An array of supported analyzers.</returns>
        private async static Task<Analyzer[]> ListAnalyzers()
        {
            try
            {
                var listAnalyzersAsync = await Client.ListAnalyzersAsync();
                return listAnalyzersAsync;
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to gather list of analyzers", exception as ClientException);
            }
        }

        /// <summary>
        /// Analyze text synchronously.
        /// </summary>
        /// <param name="request">Analyze text request.</param>
        /// <returns>An array of analyze text result.</returns>
        private async static Task<AnalyzeTextResult[]> AnalyzeText(AnalyzeTextRequest request)
        {
            try
            {
                return await Client.AnalyzeTextAsync(request);
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to analyze text", exception as ClientException);
            }
        }
    }
}