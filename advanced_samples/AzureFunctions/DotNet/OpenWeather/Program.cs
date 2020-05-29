using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenWeather;
using OSIsoft.Data.Http;
using OSIsoft.Identity;
using OSIsoft.Omf;
using OSIsoft.Omf.Converters;
using OSIsoft.OmfIngress;

namespace OpenWeatherFunction
{
    public static class Program
    {
        private static readonly string _openWeatherUrl = Environment.GetEnvironmentVariable("OPEN_WEATHER_URL");
        private static readonly string _openWeatherApiKey = Environment.GetEnvironmentVariable("OPEN_WEATHER_API_KEY");
        private static readonly string _openWeatherQueries = Environment.GetEnvironmentVariable("OPEN_WEATHER_QUERIES");
        private static readonly string _ocsUrl = Environment.GetEnvironmentVariable("OCS_URL");
        private static readonly string _ocsTenantId = Environment.GetEnvironmentVariable("OCS_TENANT_ID");
        private static readonly string _ocsNamespaceId = Environment.GetEnvironmentVariable("OCS_NAMESPACE_ID");
        private static readonly string _ocsClientId = Environment.GetEnvironmentVariable("OCS_CLIENT_ID");
        private static readonly string _ocsClientSecret = Environment.GetEnvironmentVariable("OCS_CLIENT_SECRET");

        private static IOmfIngressService _omfIngressService;

        [FunctionName("CurrentWeather")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            // Set up OMF Ingress Service
            _omfIngressService = ConfigureOcsOmf(_ocsUrl, _ocsTenantId, _ocsNamespaceId, _ocsClientId, _ocsClientSecret);

            // Send OMF Type message
            SendOmfMessage(_omfIngressService, OmfMessageCreator.CreateTypeMessage(typeof(CurrentWeather)));

            // Prepare OMF containers
            var typeId = ClrToOmfTypeConverter.Convert(typeof(CurrentWeather)).Id;
            var containers = new List<OmfContainer>();
            var data = new List<OmfDataContainer>();

            var queries = _openWeatherQueries.Split('|');
            foreach (var query in queries)
            {
                // Get Current Weather Data
                var response = JsonConvert.DeserializeObject<JObject>(HttpGet($"{_openWeatherUrl}?q={query}&appid={_openWeatherApiKey}"));

                // Parse data into OMF messages
                var value = new CurrentWeather(response);
                var streamId = $"OpenWeather_Current_{value.Name}";
                containers.Add(new OmfContainer(streamId, typeId));
                var omfValue = (OmfObjectValue)ClrToOmfValueConverter.Convert(value);
                data.Add(new OmfDataContainer(streamId, new List<OmfObjectValue>() { omfValue }));
            }

            SendOmfMessage(_omfIngressService, new OmfContainerMessage(containers));
            log.LogInformation($"Sent {containers.Count} containers");
            SendOmfMessage(_omfIngressService, new OmfDataMessage(data));
            log.LogInformation($"Sent {data.Count} data messages");
        }

        /// <summary>
        /// Configure OCS/OMF Services
        /// </summary>
        private static IOmfIngressService ConfigureOcsOmf(string address, string tenantId, string namespaceId, string clientId, string clientSecret)
        {
            var deviceAuthenticationHandler = new AuthenticationHandler(new Uri(address), clientId, clientSecret);
            var deviceBaseOmfIngressService = new OmfIngressService(new Uri(address), null, HttpCompressionMethod.None, deviceAuthenticationHandler);
            return deviceBaseOmfIngressService.GetOmfIngressService(tenantId, namespaceId);
        }

        /// <summary>
        /// Runs a generic HTTP GET request against the GitHub API
        /// </summary>
        private static string HttpGet(string url)
        {
            var uri = new Uri(url);
            var request = (HttpWebRequest)WebRequest.Create(uri);
            using var response = (HttpWebResponse)request.GetResponse();
            using var stream = response.GetResponseStream();
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Sends a message to the OCS OMF endpoint
        /// </summary>
        private static object SendOmfMessage(IOmfIngressService service, OmfMessage omfMessage)
        {
            var serializedOmfMessage = OmfMessageSerializer.Serialize(omfMessage);
            return service.SendOmfMessageAsync(serializedOmfMessage).Result;
        }
    }
}
