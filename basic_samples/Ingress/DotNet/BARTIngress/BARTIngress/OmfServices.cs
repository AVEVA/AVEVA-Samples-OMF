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
        static IOmfIngressService PiOmfIngressService { get; set; }

        /// <summary>
        /// Configure OCS OMF Ingress Service
        /// </summary>
        internal static void ConfigureOcsOmfIngress(string uri, string tenantId, string namespaceId, string clientId, string clientSecret)
        {
            var deviceAuthenticationHandler = new AuthenticationHandler(new Uri(uri), clientId, clientSecret);
            var deviceBaseOmfIngressService = new OmfIngressService(new Uri(uri), null, HttpCompressionMethod.GZip, deviceAuthenticationHandler);
            OcsOmfIngressService = deviceBaseOmfIngressService.GetOmfIngressService(tenantId, namespaceId);
        }

        /// <summary>
        /// Configure EDS OMF Ingress Service
        /// </summary>
        internal static void ConfigureEdsOmfIngress(int port = 5590)
        {
            var deviceBaseOmfIngressService = new OmfIngressService(new Uri($"http://localhost:{port}"), null, HttpCompressionMethod.GZip);
            OcsOmfIngressService = deviceBaseOmfIngressService.GetOmfIngressService("default", "default");
        }

        /// <summary>
        /// Configure PI Web API OMF Ingress Service
        /// </summary>
        internal static void ConfigurePiOmfIngress(string uri)
        {
            var deviceBaseOmfIngressService = new OmfIngressService(new Uri(uri), null, HttpCompressionMethod.GZip);
            PiOmfIngressService = deviceBaseOmfIngressService.GetOmfIngressService(null, null);
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

            if (PiOmfIngressService != null)
            {
                PiOmfIngressService.SendOMFMessageAsync(serializedOmfMessage);
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
