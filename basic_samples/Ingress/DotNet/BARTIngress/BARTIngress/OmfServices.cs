using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OSIsoft.Omf;
using OSIsoft.Omf.Converters;
using OSIsoft.OmfIngress;
using OSIsoft.Data.Http;
using OSIsoft.Identity;

namespace BARTIngress
{
    static class OmfServices
    {
        static IOmfIngressService OcsOmfIngressService { get; set; }
        static IOmfIngressService EdsOmfIngressService { get; set; }
        static HttpClient PiHttpClient { get; set; }
        static Uri PiOmfUri { get; set; }

        /// <summary>
        /// Configure OCS OMF Ingress Service
        /// </summary>
        internal static void ConfigureOcsOmfIngress(string uriString, string tenantId, string namespaceId, string clientId, string clientSecret)
        {
            var uri = new Uri(uriString);
            var authHandler = new AuthenticationHandler(uri, clientId, clientSecret);
            var omfIngressService = new OmfIngressService(uri, null, HttpCompressionMethod.GZip, authHandler);
            OcsOmfIngressService = omfIngressService.GetOmfIngressService(tenantId, namespaceId);
        }

        /// <summary>
        /// Configure EDS OMF Ingress Service
        /// </summary>
        internal static void ConfigureEdsOmfIngress(int port = 5590)
        {
            var omfIngressService = new OmfIngressService(new Uri($"http://localhost:{port}"), null, HttpCompressionMethod.GZip);
            EdsOmfIngressService = omfIngressService.GetOmfIngressService("default", "default");
        }

        /// <summary>
        /// Configure EDS PI Web API HttpClient
        /// </summary>
        /// <param name="uriString"></param>
        internal static void ConfigurePiOmfIngress(string uriString)
        {
            PiHttpClient = new HttpClient();
            PiOmfUri = new Uri(uriString + $"/omf");
        }
        
        /// <summary>
        /// Sends a message to an OMF endpoint
        /// </summary>
        internal static void SendOmfMessage(OmfMessage omfMessage)
        {
            var serializedOmfMessage = OmfMessageSerializer.Serialize(omfMessage);

            if (OcsOmfIngressService != null)
            {
                OcsOmfIngressService.SendOmfMessageAsync(serializedOmfMessage);
            }

            if (EdsOmfIngressService != null)
            {
                EdsOmfIngressService.SendOmfMessageAsync(serializedOmfMessage);
            }

            if (PiHttpClient != null)
            {
                _ = SendPiOmfMessageAsync(serializedOmfMessage);
            }
        }

        internal static async Task<string> SendPiOmfMessageAsync(SerializedOmfMessage omfMessage)
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = PiOmfUri,
                Content = new StringContent(omfMessage.ToString(), Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Accept", "application/json");
            var response = await PiHttpClient.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error sending OMF to PI Web API. Response code: {response.StatusCode} Response: {responseString}");
            return responseString;
        }

        internal static void SendOmfData<T>(Dictionary<string, T> data, string typeId)
        {
            var containers = new List<OmfContainer>();
            var dataContainers = new List<OmfDataContainer>();
            foreach (var streamId in data.Keys)
            {
                containers.Add(new OmfContainer(streamId, typeId));
                var omfValue = (OmfObjectValue)ClrToOmfValueConverter.Convert(data[streamId]);
                dataContainers.Add(new OmfDataContainer(streamId, new List<OmfObjectValue>() { omfValue }));
            }

            // Send file data
            SendOmfMessage(new OmfContainerMessage(containers));
            SendOmfMessage(new OmfDataMessage(dataContainers));
        }
    }
}
