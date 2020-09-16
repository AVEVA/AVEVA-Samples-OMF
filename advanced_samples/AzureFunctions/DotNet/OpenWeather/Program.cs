using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OSIsoft.Data.Http;
using OSIsoft.Identity;
using OSIsoft.Omf;
using OSIsoft.Omf.Converters;
using OSIsoft.OmfIngress;

namespace OpenWeather
{
    public static class Program
    {
        private static IOmfIngressService _omfIngressService;
        private static ILogger _log;

        public static AppSettings Settings { get; set; }

        [FunctionName("CurrentWeather")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            _log = log;
            LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            LoadConfiguration();

            if (string.IsNullOrEmpty(Settings.OpenWeatherKey))
            {
                LogInformation("No OpenWeather API Key provided, function will generate random data");
            }

            // Set up OMF Ingress Service
            _omfIngressService = ConfigureOcsOmf(Settings.OcsUri, Settings.OcsTenantId, Settings.OcsNamespaceId, Settings.OcsClientId, Settings.OcsClientSecret);

            // Send OMF Type message
            SendOmfMessage(_omfIngressService, OmfMessageCreator.CreateTypeMessage(typeof(CurrentWeather)));

            // Prepare OMF containers
            var typeId = ClrToOmfTypeConverter.Convert(typeof(CurrentWeather)).Id;
            var containers = new List<OmfContainer>();
            var data = new Dictionary<string, IEnumerable<CurrentWeather>>();

            var queries = Settings.OpenWeatherQueries.Split('|');
            foreach (var query in queries)
            {
                if (!string.IsNullOrEmpty(Settings.OpenWeatherKey))
                {
                    // Get Current Weather Data
                    var response = JsonConvert.DeserializeObject<JObject>(HttpGet($"{Settings.OpenWeatherUri}?q={query}&appid={Settings.OpenWeatherKey}"));

                    // Parse data into OMF messages
                    var value = new CurrentWeather(response);
                    var streamId = $"OpenWeather_Current_{value.Name}";
                    containers.Add(new OmfContainer(streamId, typeId));
                    data.Add(streamId, new CurrentWeather[] { value });
                }
                else
                {
                    // No key provided, generate random data
                    containers.Add(new OmfContainer(query, typeId));
                    var value = new CurrentWeather(query);
                    data.Add(query, new CurrentWeather[] { value });
                }
            }

            SendOmfMessage(_omfIngressService, new OmfContainerMessage(containers));
            LogInformation($"Sent {containers.Count} containers");
            SendOmfMessage(_omfIngressService, OmfMessageCreator.CreateDataMessage(data));
            LogInformation($"Sent {data.Count} data messages");
        }

        private static void LogInformation(string message)
        {
            if (_log != null)
            {
                _log.LogInformation(message);
            }
        }

        private static void LoadConfiguration()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "/appsettings.json"))
            {
                // Running locally, read configuration from file
                Settings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(Directory.GetCurrentDirectory() + "/appsettings.json"));
            }
            else
            {
                // Running in Azure Function, read configuration from Environment
                Settings = new AppSettings()
                {
                    OpenWeatherUri = new Uri(Environment.GetEnvironmentVariable("OPEN_WEATHER_URI")),
                    OpenWeatherKey = Environment.GetEnvironmentVariable("OPEN_WEATHER_KEY"),
                    OpenWeatherQueries = Environment.GetEnvironmentVariable("OPEN_WEATHER_QUERIES"),
                    OcsUri = new Uri(Environment.GetEnvironmentVariable("OCS_URI")),
                    OcsTenantId = Environment.GetEnvironmentVariable("OCS_TENANT_ID"),
                    OcsNamespaceId = Environment.GetEnvironmentVariable("OCS_NAMESPACE_ID"),
                    OcsClientId = Environment.GetEnvironmentVariable("OCS_CLIENT_ID"),
                    OcsClientSecret = Environment.GetEnvironmentVariable("OCS_CLIENT_SECRET"),
                };
            }
        }

        /// <summary>
        /// Configure OCS/OMF Services
        /// </summary>
        private static IOmfIngressService ConfigureOcsOmf(Uri address, string tenantId, string namespaceId, string clientId, string clientSecret)
        {
            var deviceAuthenticationHandler = new AuthenticationHandler(address, clientId, clientSecret);
            var deviceBaseOmfIngressService = new OmfIngressService(address, null, HttpCompressionMethod.None, deviceAuthenticationHandler);
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
