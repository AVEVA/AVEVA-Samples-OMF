using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using OSIsoft.Data.Http;
using OSIsoft.Identity;
using OSIsoft.Omf;
using OSIsoft.Omf.Converters;
using OSIsoft.OmfIngress;

namespace BartIngress
{
    /// <summary>
    /// Manages sending OMF data to the OCS, EDS, and/or PI Web API OMF endpoints
    /// </summary>
    public class OmfServices : IDisposable
    {
        public IOmfIngressService OcsOmfIngressService { get; set; }
        public IOmfIngressService EdsOmfIngressService { get; set; }
        public HttpClient PiHttpClient { get; set; }
        private HttpClientHandler PiHttpClientHandler { get; set; }
        private Uri PiOmfUri { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Configure OCS OMF Ingress Service
        /// </summary>
        /// <param name="ocsUri">OSIsoft Cloud Services OMF Endpoint URI</param>
        /// <param name="tenantId">OSIsoft Cloud Services Tenant ID</param>
        /// <param name="namespaceId">OSIsoft Cloud Services Namespace ID</param>
        /// <param name="clientId">OSIsoft Cloud Services Client ID</param>
        /// <param name="clientSecret">OSIsoft Cloud Services Client Secret</param>
        internal void ConfigureOcsOmfIngress(Uri ocsUri, string tenantId, string namespaceId, string clientId, string clientSecret)
        {
            var authHandler = new AuthenticationHandler(ocsUri, clientId, clientSecret);
            var omfIngressService = new OmfIngressService(ocsUri, null, HttpCompressionMethod.GZip, authHandler);
            OcsOmfIngressService = omfIngressService.GetOmfIngressService(tenantId, namespaceId);
        }

        /// <summary>
        /// Configure EDS OMF Ingress Service
        /// </summary>
        /// <param name="port">Edge Data Store Port, default is 5590</param>
        internal void ConfigureEdsOmfIngress(int port = 5590)
        {
            var omfIngressService = new OmfIngressService(new Uri($"http://localhost:{port}"), null, HttpCompressionMethod.GZip);
            EdsOmfIngressService = omfIngressService.GetOmfIngressService("default", "default");
        }

        /// <summary>
        /// Configure PI OMF HttpClient
        /// </summary>
        /// <param name="piUri">PI Web API Endpoint URI, like https://server//piwebapi</param>
        /// <param name="username">Domain user name to use for Basic authentication against PI Web API</param>
        /// <param name="password">Domain user password to use for Basic authentication against PI Web API</param>
        /// <param name="validate">Whether to validate the PI Web API endpoint certificate. Setting to false should only be done for testing with a self-signed PI Web API certificate as it is insecure.</param>
        internal void ConfigurePiOmfIngress(Uri piUri, string username, string password, bool validate = true)
        {
            PiHttpClientHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential(username, password),
            };            
            if (!validate)
            {
                Console.WriteLine(Resources.WARNING_CERT_VALIDATE_DISABLED);

                // This turns off SSL verification
                // This should not be done in production, please properly handle your certificates
                PiHttpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            }

            PiHttpClient = new HttpClient(PiHttpClientHandler);
            PiOmfUri = new Uri(piUri.AbsoluteUri + $"/omf");
        }

        /// <summary>
        /// Sends a dictionary of OMF data keyed by the stream ID to the configured OMF endpoints
        /// </summary>
        /// <typeparam name="T">OMF type of the OMF data to be sent</typeparam>
        /// <param name="data">Dictionary of OMF data keyed by the stream ID</param>
        /// <param name="typeId">TypeID of the OMF type</param>
        internal void SendOmfData<T>(Dictionary<string, T> data, string typeId)
        {
            var containers = new List<OmfContainer>();
            var dataContainers = new List<OmfDataContainer>();
            foreach (var streamId in data.Keys)
            {
                containers.Add(new OmfContainer(streamId, typeId));
                var omfValue = (OmfObjectValue)ClrToOmfValueConverter.Convert(data[streamId]);
                dataContainers.Add(new OmfDataContainer(streamId, new List<OmfObjectValue>() { omfValue }));
            }

            // Send parsed data
            SendOmfMessage(new OmfContainerMessage(containers));
            SendOmfMessage(new OmfDataMessage(dataContainers));
        }

        /// <summary>
        /// Sends a message to the configured OMF endpoints
        /// </summary>
        /// <param name="omfMessage">The OMF message to send</param>
        internal void SendOmfMessage(OmfMessage omfMessage)
        {
            var serializedOmfMessage = OmfMessageSerializer.Serialize(omfMessage);

            if (OcsOmfIngressService != null)
            {
                _ = OcsOmfIngressService.SendOmfMessageAsync(serializedOmfMessage).Result;
            }

            if (EdsOmfIngressService != null)
            {
                _ = EdsOmfIngressService.SendOmfMessageAsync(serializedOmfMessage).Result;
            }

            if (PiHttpClient != null)
            {
                _ = SendPiOmfMessageAsync(serializedOmfMessage).Result;
            }
        }

        protected virtual void Dispose(bool includeManaged)
        {
            if (includeManaged)
            {
                if (PiHttpClientHandler != null)
                {
                    PiHttpClientHandler.Dispose();
                }
            }
        }

        /// <summary>
        /// Sends an OMF message to the configured PI Web API endpoint
        /// </summary>
        /// <param name="omfMessage">The OMF message to send</param>
        /// <returns>A task returning the response of the HTTP request</returns>
        private async Task<string> SendPiOmfMessageAsync(SerializedOmfMessage omfMessage)
        {
            using var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = PiOmfUri,
                Content = new ByteArrayContent(omfMessage.BodyBytes),
            };
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("X-Requested-With", "XMLHTTPRequest");
            foreach (var omfHeader in omfMessage.Headers)
            {
                request.Headers.Add(omfHeader.Name, omfHeader.Value);
            }

            var response = await PiHttpClient.SendAsync(request).ConfigureAwait(false);

            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error sending OMF to PI Web API. Response code: {response.StatusCode} Response: {responseString}");
            return responseString;
        }
    }
}
