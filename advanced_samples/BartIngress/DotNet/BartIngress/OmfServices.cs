using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OSIsoft.Omf;
using OSIsoft.Omf.Converters;

namespace BartIngress
{
    /// <summary>
    /// Manages sending OMF data to the OCS, EDS, and/or PI Web API OMF endpoints
    /// </summary>
    public class OmfServices : IDisposable
    {
        private OmfMessage _typeDeleteMessage;
        private OmfMessage _containerDeleteMessage;

        /// <summary>
        /// Creates a new instance of OMF Services
        /// </summary>
        /// <param name="validate">Whether to validate the PI Web API endpoint certificate. Setting to false should only be done for testing with a self-signed PI Web API certificate as it is insecure.</param>
        public OmfServices(bool validate = true)
        {
            if (validate)
            {
                HttpClient = new HttpClient();
            }
            else
            {
                Console.WriteLine(Resources.WARNING_CERT_VALIDATE_DISABLED);

                HttpClientHandler = new HttpClientHandler
                {
                    // This turns off SSL verification
                    // This should not be done in production, please properly handle your certificates
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
                };
                HttpClient = new HttpClient(HttpClientHandler);
            }

            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        }

        public HttpClient HttpClient { get; set; }
        private HttpClientHandler HttpClientHandler { get; set; }

        private Uri OcsOmfUri { get; set; }
        private AuthenticationHeaderValue OcsAuthHeader { get; set; }
        private Uri EdsOmfUri { get; set; }
        private Uri PiOmfUri { get; set; }
        private AuthenticationHeaderValue PiAuthHeader { get; set; }

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
            OcsOmfUri = new Uri(ocsUri.AbsoluteUri + $"api/v1/tenants/{tenantId}/namespaces/{namespaceId}/omf");
            var token = GetOCSClientCredentialsToken(ocsUri, clientId, clientSecret);
            OcsAuthHeader = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Configure EDS OMF Ingress Service
        /// </summary>
        /// <param name="port">Edge Data Store Port, default is 5590</param>
        internal void ConfigureEdsOmfIngress(int port = 5590)
        {
            EdsOmfUri = new Uri($"http://localhost:{port}/api/v1/tenants/default/namespaces/default/omf");
        }

        /// <summary>
        /// Configure PI OMF HttpClient
        /// </summary>
        /// <param name="piUri">PI Web API Endpoint URI, like https://server//piwebapi</param>
        /// <param name="username">Domain user name to use for Basic authentication against PI Web API</param>
        /// <param name="password">Domain user password to use for Basic authentication against PI Web API</param>
        internal void ConfigurePiOmfIngress(Uri piUri, string username, string password)
        {
            PiOmfUri = new Uri(piUri.AbsoluteUri + $"/omf");
            var authBytes = Encoding.ASCII.GetBytes($"{username}:{password}");
            PiAuthHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));
        }

        /// <summary>
        /// Sends OMF type message for a type
        /// </summary>
        /// <param name="type">OMF type to be sent</param>
        internal void SendOmfType(Type type)
        {
            var msg = OmfMessageCreator.CreateTypeMessage(type);
            SendOmfMessage(msg);
            msg.ActionType = ActionType.Delete;
            _typeDeleteMessage = msg;
        }

        /// <summary>
        /// Sends OMF container messages for a dictionary of OMF data keyed by the stream ID to the configured OMF endpoints
        /// </summary>
        /// <typeparam name="T">OMF type of the OMF data to be sent</typeparam>
        /// <param name="data">Dictionary of OMF data keyed by the stream ID</param>
        /// <param name="typeId">TypeID of the OMF type</param>
        internal void SendOmfContainers<T>(Dictionary<string, T> data, string typeId)
        {
            var containers = new List<OmfContainer>();
            foreach (var streamId in data.Keys)
            {
                containers.Add(new OmfContainer(streamId, typeId));
            }

            var msg = new OmfContainerMessage(containers);
            SendOmfMessage(msg);
            msg.ActionType = ActionType.Delete;
            _containerDeleteMessage = msg;
        }

        /// <summary>
        /// Sends OMF data messages for a dictionary of OMF data keyed by the stream ID to the configured OMF endpoints
        /// </summary>
        /// <typeparam name="T">OMF type of the OMF data to be sent</typeparam>
        /// <param name="data">Dictionary of OMF data keyed by the stream ID</param>
        internal void SendOmfData<T>(Dictionary<string, T> data)
        {
            var dataContainers = new List<OmfDataContainer>();
            foreach (var streamId in data.Keys)
            {
                var omfValue = (OmfObjectValue)ClrToOmfValueConverter.Convert(data[streamId]);
                dataContainers.Add(new OmfDataContainer(streamId, new List<OmfObjectValue>() { omfValue }));
            }

            SendOmfMessage(new OmfDataMessage(dataContainers));
        }

        /// <summary>
        /// Sends a message to the configured OMF endpoints
        /// </summary>
        /// <param name="omfMessage">The OMF message to send</param>
        internal void SendOmfMessage(OmfMessage omfMessage)
        {
            var serializedOmfMessage = OmfMessageSerializer.Serialize(omfMessage);

            if (OcsOmfUri != null)
            {
                _ = SendOmfMessageAsync(serializedOmfMessage, OcsOmfUri, OcsAuthHeader).Result;
            }

            if (EdsOmfUri != null)
            {
                _ = SendOmfMessageAsync(serializedOmfMessage, EdsOmfUri).Result;
            }

            if (PiOmfUri != null)
            {
                _ = SendOmfMessageAsync(serializedOmfMessage, PiOmfUri, PiAuthHeader).Result;
            }
        }

        /// <summary>
        /// Deletes type and containers that were created by these services
        /// </summary>
        internal void CleanupOmf()
        {
            var serializedTypeDelete = OmfMessageSerializer.Serialize(_typeDeleteMessage);
            var serializedContainerDelete = OmfMessageSerializer.Serialize(_containerDeleteMessage);

            if (OcsOmfUri != null)
            {
                _ = SendOmfMessageAsync(serializedContainerDelete, OcsOmfUri, OcsAuthHeader).Result;
                _ = SendOmfMessageAsync(serializedTypeDelete, OcsOmfUri, OcsAuthHeader).Result;
            }

            if (EdsOmfUri != null)
            {
                _ = SendOmfMessageAsync(serializedContainerDelete, EdsOmfUri).Result;
                _ = SendOmfMessageAsync(serializedTypeDelete, EdsOmfUri).Result;
            }

            if (PiOmfUri != null)
            {
                _ = SendOmfMessageAsync(serializedContainerDelete, PiOmfUri).Result;
                _ = SendOmfMessageAsync(serializedTypeDelete, PiOmfUri).Result;
            }
        }

        protected virtual void Dispose(bool includeManaged)
        {
            if (includeManaged)
            {
                if (HttpClientHandler != null)
                {
                    HttpClientHandler.Dispose();
                }
            }
        }

        /// <summary>
        /// Sends an OMF message to an OMF endpoint with optional authentication header
        /// </summary>
        /// <param name="omfMessage">The OMF message to send</param>
        /// <param name="omfUri">The OMF endpoint to send to</param>
        /// <param name="authHeader">(Optional) The authentication header to add to the request</param>
        /// <returns>A task returning the response of the HTTP request</returns>
        private async Task<string> SendOmfMessageAsync(SerializedOmfMessage omfMessage, Uri omfUri, AuthenticationHeaderValue authHeader = null)
        {
            using var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = omfUri,
                Content = new ByteArrayContent(omfMessage.BodyBytes),
            };

            if (authHeader != null)
            {
                request.Headers.Authorization = authHeader;
            }

            foreach (var omfHeader in omfMessage.Headers)
            {
                request.Headers.Add(omfHeader.Name, omfHeader.Value);
            }

            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error sending OMF to endpoint at {omfUri}. Response code: {response.StatusCode} Response: {responseString}");
            return responseString;
        }

        /// <summary>
        /// Gets a bearer token from OCS using Client Credentials
        /// </summary>
        /// <param name="ocsUri">OSIsoft Cloud Services OMF Endpoint URI</param>
        /// <param name="clientId">OSIsoft Cloud Services Client ID</param>
        /// <param name="clientSecret">OSIsoft Cloud Services Client Secret</param>
        /// <returns>A bearer token</returns>
        private string GetOCSClientCredentialsToken(Uri ocsUri, string clientId, string clientSecret)
        {
            using var openIdConfigRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(ocsUri.AbsoluteUri + "identity/.well-known/openid-configuration"),
            };
            var openIdConfigResponse = HttpClient.SendAsync(openIdConfigRequest).Result;
            var openIdConfig = JsonConvert.DeserializeObject<JObject>(openIdConfigResponse.Content.ReadAsStringAsync().Result);
            var token_endpoint = (string)openIdConfig["token_endpoint"];

            using var tokenRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(token_endpoint),
                Content = new FormUrlEncodedContent(
                    new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("grant_type", "client_credentials"),
                        new KeyValuePair<string, string>("client_id", clientId),
                        new KeyValuePair<string, string>("client_secret", clientSecret),
                        new KeyValuePair<string, string>("resource", ocsUri.AbsoluteUri),
                    }),
            };

            var tokenResponse = HttpClient.SendAsync(tokenRequest).Result;
            var token = JsonConvert.DeserializeObject<JObject>(tokenResponse.Content.ReadAsStringAsync().Result);
            return (string)token["access_token"];
        }
    }
}
