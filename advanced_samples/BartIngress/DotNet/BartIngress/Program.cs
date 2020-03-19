using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using OSIsoft.Omf;
using OSIsoft.Omf.Converters;

namespace BartIngress
{
    public static class Program
    {
        private static readonly object _timerLock = new object();
        private static readonly string _typeId = ClrToOmfTypeConverter.Convert(typeof(BartStationEtd)).Id;
        private static Timer _timer;

        public static AppSettings Settings { get; set; }
        private static int TimerInterval { get; set; } = 10000;
        private static OmfServices OmfServices { get; set; } = new OmfServices();

        public static void Main()
        {
            LoadConfiguration();
            _timer = new Timer(new TimerCallback(TimerTask), null, 0, 10000);
            Console.WriteLine(Resources.MSG_STARTED);
            Console.ReadLine();
        }
        
        /// <summary>
        /// Loads configuration from the appsettings.json file and sets up configured OMF endpoints
        /// </summary>
        public static void LoadConfiguration()
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
                OmfServices.ConfigurePiOmfIngress(Settings.PiWebApiUri, Settings.Username, Settings.Password, Settings.ValidateEndpointCertificate);
            }

            OmfServices.SendOmfMessage(OmfMessageCreator.CreateTypeMessage(typeof(BartStationEtd)));
        }

        /// <summary>
        /// Run BART API ingress to configured OMF endpoints
        /// </summary>
        public static void RunIngress()
        {
            var data = BartApi.GetRealTimeEstimates(Settings.BartApiKey, Settings.BartApiOrig, Settings.BartApiDest);
            OmfServices.SendOmfData(data, _typeId);
            Console.WriteLine($"{DateTime.Now}: Sent value for {data.Keys.Count} stream{(data.Keys.Count > 1 ? "s" : string.Empty)}");
        }

        /// <summary>
        /// Task callback when the timer fires, attempts to run ingress 
        /// </summary>
        private static void TimerTask(object timerState)
        {
            var hasLock = false;

            try
            {
                Monitor.TryEnter(_timerLock, ref hasLock);
                if (!hasLock)
                {
                    return;
                }

                _timer.Change(Timeout.Infinite, Timeout.Infinite);

                RunIngress();
            }
            finally
            {
                if (hasLock)
                {
                    Monitor.Exit(_timerLock);
                    _timer.Change(TimerInterval, TimerInterval);
                }
            }
        }
    }
}
