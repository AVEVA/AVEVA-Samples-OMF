using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BartIngress
{
    public static class BartApi
    {
        /// <summary>
        /// Gets and parses the current data from the BART ETD (Estimated Time of Departure) API
        /// </summary>
        /// <param name="key">BART API registration key</param>
        /// <param name="orig">Specifies the origin station abbreviation. The default value, "all", will get all current ETDs.</param>
        /// <param name="dest">Specifies the destination station abbreviation. The default value, "all", will parse all current destination ETDs.</param>
        /// <returns>A dictionary of ETD data keyed by the stream ID</returns>
        internal static Dictionary<string, IEnumerable<BartStationEtd>> GetRealTimeEstimates(string key, string orig = "all", string dest = "all")
        {
            var data = new Dictionary<string, IEnumerable<BartStationEtd>>();
            var etdJson = HttpGet(key, orig);
            var etdRoot = JsonConvert.DeserializeObject<JObject>(etdJson)["root"];
            var date = (string)etdRoot["date"];
            var time = (string)etdRoot["time"];
            time = time.Replace("PST", "-8:00", StringComparison.OrdinalIgnoreCase).Replace("PDT", "-7:00", StringComparison.OrdinalIgnoreCase);
            var dateTime = DateTime.ParseExact(date + " " + time, "MM/dd/yyyy hh:mm:ss tt zzz", CultureInfo.InvariantCulture).ToUniversalTime();
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
                        data.Add(streamId, new BartStationEtd[] { stationEtd });
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// Runs an HTTP Get request against the BART ETD (Estimated Time of Departure) API
        /// <param name="key">BART API registration key</param>
        /// <param name="orig">Specifies the origin station abbreviation. The default value, "all", will get all current ETDs.</param>
        /// </summary>
        private static string HttpGet(string key, string orig = "all")
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
