using System;
using BartIngress;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OSIsoft.Data;
using OSIsoft.Data.Http;
using OSIsoft.Identity;
using Xunit;

namespace BartIngressTests
{
    public class BartIngressE2ETests
    {
        [Fact]
        public void BartIngressEndToEndTest()
        {
            Program.LoadConfiguration();

            // Verify timestamp is within last minute
            var verifyTimestamp = DateTime.UtcNow.AddMinutes(-1);

            // Test requires that specific stations are chosen for BartApiOrig and BartApiDest, "all" is not allowed
            var streamId = $"BART_{Program.Settings.BartApiOrig}_{Program.Settings.BartApiDest}";

            Program.RunIngress();

            // Verify OCS Data
            using var ocsAuthenticationHandler = new AuthenticationHandler(Program.Settings.OcsUri, Program.Settings.OcsClientId, Program.Settings.OcsClientSecret);
            var ocsSdsService = new SdsService(Program.Settings.OcsUri, null, HttpCompressionMethod.GZip, ocsAuthenticationHandler);
            var ocsDataService = ocsSdsService.GetDataService(Program.Settings.OcsTenantId, Program.Settings.OcsNamespaceId);
            var ocsValue = ocsDataService.GetLastValueAsync<BartStationEtd>(streamId).Result;
            Assert.True(ocsValue.TimeStamp > verifyTimestamp);

            // Verify EDS Data
            var edsSdsService = new SdsService(new Uri($"http://localhost:{Program.Settings.EdsPort}"), null, HttpCompressionMethod.GZip, null);
            var edsDataService = edsSdsService.GetDataService("default", "default");
            var edsValue = edsDataService.GetLastValueAsync<BartStationEtd>(streamId).Result;
            Assert.True(edsValue.TimeStamp > verifyTimestamp);

            // Verify PI Web API Data
            var piPointUri = new Uri($"{Program.Settings.PiWebApiUri}/points?path=\\\\{Program.Settings.TestPiDataArchive}\\{streamId}.Minutes");
            var piPoint = JsonConvert.DeserializeObject<JObject>(Program.OmfServices.PiHttpClient.GetStringAsync(piPointUri).Result);
            var piWebId = piPoint["WebId"];
            var piValueUri = new Uri($"{Program.Settings.PiWebApiUri}/streams/{piWebId}/value");
            var piValue = JsonConvert.DeserializeObject<JObject>(Program.OmfServices.PiHttpClient.GetStringAsync(piValueUri).Result);
            var piTimestamp = (DateTime)piValue["Timestamp"];
            Assert.True(piTimestamp > verifyTimestamp);
        }
    }
}
