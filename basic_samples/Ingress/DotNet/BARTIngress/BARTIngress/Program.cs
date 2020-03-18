using System;
using System.Net;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using OSIsoft.Omf;
using OSIsoft.Omf.Converters;

namespace BARTIngress
{
    class Program
    {
        static AppSettings Settings { get; set; }
        static int TimerInterval { get; set; } = 10000;

        static readonly object timerLock = new object();
        static Timer timer;
        static string typeId = ClrToOmfTypeConverter.Convert(typeof(BartStationEtd)).Id;

        static void Main(string[] args)
        {
            LoadConfiguration();
            timer = new Timer(new TimerCallback(TimerTask), null, 0, 10000);
            Console.WriteLine("Started, press Enter to quit");
            Console.ReadLine();
        }
        
        private static void LoadConfiguration()
        {
            Settings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(Directory.GetCurrentDirectory() + "\\appsettings.json"));

            if (Settings.SendToOcs)
            {
                OmfServices.ConfigureOcsOmfIngress(Settings.OcsUri, Settings.OcsTenantId, Settings.OcsNamespaceId, Settings.OcsClientId, Settings.OcsClientSecret);
            }

            if (Settings.SendToEds)
            {
                OmfServices.ConfigureEdsOmfIngress(Settings.EdsPort);
            }

            if (Settings.SendToPi)
            {
                OmfServices.ConfigurePiOmfIngress(Settings.PiWebApiUri);
            }

            if (!Settings.ValidateEndpointCertificate)
            {
                Console.WriteLine("Warning: You have disabled verification of destination certificates. This should only be done for testing with self-signed certificate as this introduces security issues.");
                // This turns off SSL verification
                // This should not be done in production, please properly handle your certificates
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            }

            OmfServices.SendOmfMessage(OmfMessageCreator.CreateTypeMessage(typeof(BartStationEtd)));
        }

        private static void TimerTask(object timerState)
        {
            var hasLock = false;

            try
            {
                Monitor.TryEnter(timerLock, ref hasLock);
                if (!hasLock)
                {
                    return;
                }

                timer.Change(Timeout.Infinite, Timeout.Infinite);

                RunIngress();
            }
            finally
            {
                if (hasLock)
                {
                    Monitor.Exit(timerLock);
                    timer.Change(TimerInterval, TimerInterval);
                }
            }
        }

        private static void RunIngress()
        {
            var data = BartApi.GetRealTimeEstimates(Settings.BartApiOrig, Settings.BartApiDest, Settings.BartApiKey);
            OmfServices.SendOmfData(data, typeId);
            Console.WriteLine($"{DateTime.Now.ToString()}: Sent value for {data.Keys.Count} stream{(data.Keys.Count > 1 ? "s" : string.Empty)}");
        }
    }
}
