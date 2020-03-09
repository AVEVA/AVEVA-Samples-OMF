using System;
using System.Collections.Generic;
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
        /// Sends a message to an OMF endpoint
        /// </summary>
        internal static void SendOmfMessage(OmfMessage omfMessage)
        {
            var serializedOmfMessage = OmfMessageSerializer.Serialize(omfMessage);

            if (OcsOmfIngressService != null)
            {
                OcsOmfIngressService.SendOMFMessageAsync(serializedOmfMessage);
            }

            if (EdsOmfIngressService != null)
            {
                EdsOmfIngressService.SendOMFMessageAsync(serializedOmfMessage);
            }
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
