using System;
using System.Threading;
using BartIngress;
using OSIsoft.Data;
using OSIsoft.Data.Http;
using OSIsoft.Identity;
using Xunit;

namespace BartIngressTests
{
    public class UnitTests
    {
        [Fact]
        public void BartIngressTest()
        {
            Program.LoadConfiguration();

            // Verify timestamp is within last minute
            var verifyTimestamp = DateTime.UtcNow.AddMinutes(-1);

            // Test requires that specific stations are chosen for BartApiOrig and BartApiDest, "all" is not allowed
            var streamId = $"BART_{Program.Settings.BartApiOrig}_{Program.Settings.BartApiDest}";

            try
            {
                Program.RunIngress();

                // Wait for data to be processed by OCS
                Thread.Sleep(5000);

                // Edge Data Store and PI Web API process OMF before sending a response, and will return an error code if there is a problem
                // In this test, the call to RunIngress above will result in an exception if there is a failure on either of those endpoints

                // OCS does not validate OMF before sending a success response, so the test must check that the messages were successful
                using var ocsAuthenticationHandler = new AuthenticationHandler(Program.Settings.OcsUri, Program.Settings.OcsClientId, Program.Settings.OcsClientSecret);
                var ocsSdsService = new SdsService(Program.Settings.OcsUri, null, HttpCompressionMethod.GZip, ocsAuthenticationHandler);
                var ocsDataService = ocsSdsService.GetDataService(Program.Settings.OcsTenantId, Program.Settings.OcsNamespaceId);
                var ocsValue = ocsDataService.GetLastValueAsync<BartStationEtd>(streamId).Result;
                Assert.True(ocsValue.TimeStamp > verifyTimestamp);
            }
            finally
            {
                // Delete type and containers
                Program.Cleanup();
            }
        }
    }
}
