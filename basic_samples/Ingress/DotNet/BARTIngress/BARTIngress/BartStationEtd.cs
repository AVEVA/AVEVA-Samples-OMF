using System;
using Newtonsoft.Json.Linq;
using OSIsoft.Omf;
using OSIsoft.Omf.DefinitionAttributes;

namespace BARTIngress
{
    [OmfType(ClassificationType = ClassificationType.Dynamic, Name ="BartStationEtd", Description ="BART station current departure information")]
    public class BartStationEtd
    {
        [OmfProperty(IsIndex = true)]
        public DateTime TimeStamp { get; set; }
        public int Minutes { get; set; }
        public int Platform { get; set; }
        public string Direction { get; set; }
        public int Length { get; set; }
        public string Color { get; set; }
        public string HexColor { get; set; }
        public int BikeFlag { get; set; }
        public int Delay { get; set; }

        public BartStationEtd(DateTime timeStamp, JObject jObject)
        {
            TimeStamp = timeStamp;
            int.TryParse((string)jObject["minutes"], out int minutes);
            Minutes = minutes;
            Platform = (int)jObject["platform"];
            Direction = (string)jObject["direction"];
            Length = (int)jObject["length"];
            Color = (string)jObject["color"];
            HexColor = (string)jObject["hexcolor"];
            BikeFlag = (int)jObject["bikeflag"];
            Delay = (int)jObject["delay"];
        }
    }
}
