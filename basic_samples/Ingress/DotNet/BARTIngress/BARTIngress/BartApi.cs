using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BARTIngress
{
    static class BartApi
    {
        internal static Dictionary<string, BartStationEtd> GetRealTimeEstimates(string orig = "all", string dest = "all", string key = "MW9S-E7SL-26DU-VV8V")
        {
            var data = new Dictionary<string, BartStationEtd>();
            var etdJson = HttpGet(orig, key);
            var etdRoot = JsonConvert.DeserializeObject<JObject>(etdJson)["root"];
            var date = (string)etdRoot["date"];
            var time = (string)etdRoot["time"];
            time = time.Replace("PST", "-8:00").Replace("PDT", "-7:00");
            var dateTime = DateTime.ParseExact(date + " " + time, "MM/dd/yyyy hh:mm:ss tt zzz", CultureInfo.InvariantCulture);
            var origins = (JArray)etdRoot["station"];
            foreach (JObject origin in origins)
            {
                var origAbbr = (string)origin["abbr"];
                var destinations = (JArray)origin["etd"];
                foreach (JObject destination in destinations)
                {
                    var destAbbr = (string)destination["abbreviation"];
                    if (string.Equals(dest, "all", StringComparison.OrdinalIgnoreCase) || string.Equals(dest, destAbbr, StringComparison.OrdinalIgnoreCase))
                    {
                        var estimate = (JObject)destination["estimate"][0];
                        var stationEtd = new BartStationEtd(dateTime, estimate);
                        var streamId = $"BART_{origAbbr}_{destAbbr}";
                        data.Add(streamId, stationEtd);
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// Runs a generic HTTP GET request
        /// </summary>
        private static string HttpGet(string orig = "all", string key = "MW9S-E7SL-26DU-VV8V")
        {
            var uri = new Uri($"https://api.bart.gov/api/etd.aspx?cmd=etd&orig={orig}&key={key}&json=y");
            var request = (HttpWebRequest)WebRequest.Create(uri);
            using var response = (HttpWebResponse)request.GetResponse();
            using var stream = response.GetResponseStream();
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
