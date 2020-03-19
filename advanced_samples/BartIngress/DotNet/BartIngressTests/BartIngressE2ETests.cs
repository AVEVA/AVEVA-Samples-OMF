using System;
using System.Net.Http;
using BartIngress;
using Xunit;

namespace BartIngressTests
{
    public class BartIngressE2ETests
    {
        [Fact]
        public void BartIngressEndToEndTest()
        {
            // Verify timestamp is within last minute
            var verifyTimestamp = DateTime.UtcNow.AddMinutes(-1);

            // Test requires that specific stations are chosen for BartApiOrig and BartApiDest, "all" is not allowed
            var streamId = $"BART_{Program.Settings.BartApiOrig}_{Program.Settings.BartApiDest}";

            Program.LoadConfiguration();
            Program.RunIngress();

            // TODO: Verify OCS Data
            var uri = new Uri($"{Program.Settings.OcsUri}/api/v1/Tenants/{Program.Settings.OcsTenantId}/Namespaces/{Program.Settings.OcsTenantId}/Streams/{streamId}/Data/Last");
            var request = (HttpWebRequest)WebRequest.Create(uri);
            using var response = (HttpWebResponse)request.GetResponse();
            using var stream = response.GetResponseStream();
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();

            // TODO: Verify EDS Data

            // TODO: Verify PI Web API Data
        }
    }
}
