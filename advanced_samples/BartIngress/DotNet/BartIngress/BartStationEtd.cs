using System;
using Newtonsoft.Json.Linq;
using OSIsoft.Omf;
using OSIsoft.Omf.DefinitionAttributes;

namespace BartIngress
{
    /// <summary>
    /// OMF Definition of BART Station ETD Data
    /// </summary>
    [OmfType(ClassificationType = ClassificationType.Dynamic, Name ="BartStationEtd", Description ="BART station current departure information")]
    public class BartStationEtd
    {
        public BartStationEtd()
        {
        }

        /// <summary>
        /// Create a new data value for a BART Station ETD Stream
        /// </summary>
        /// <param name="timeStamp">The timestamp of the value</param>
        /// <param name="data">The API JSON object representing the value</param>
        public BartStationEtd(DateTime timeStamp, JObject data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            TimeStamp = timeStamp;
            var validMinutes = int.TryParse((string)data["minutes"], out int minutes);
            Minutes = validMinutes ? minutes : 0;
            Platform = (int)data["platform"];
            Direction = (string)data["direction"];
            Length = (int)data["length"];
            Color = (string)data["color"];
            HexColor = (string)data["hexcolor"];
            BikeFlag = (int)data["bikeflag"];
            Delay = (int)data["delay"];
        }

        [OmfProperty(IsIndex = true)]
        public DateTime TimeStamp { get; set; }

        [OmfProperty(Uom = "minute")]
        public int Minutes { get; set; }

        public int Platform { get; set; }

        public string Direction { get; set; }

        public int Length { get; set; }

        public string Color { get; set; }

        public string HexColor { get; set; }

        public int BikeFlag { get; set; }

        [OmfProperty(Uom = "minute")]
        public int Delay { get; set; }
    }
}
