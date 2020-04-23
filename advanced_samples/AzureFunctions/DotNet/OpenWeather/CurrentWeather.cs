using System;
using System.Collections.Generic;
using System.Text;
using OSIsoft.Omf;
using OSIsoft.Omf.DefinitionAttributes;

namespace OpenWeather
{
    [OmfType(ClassificationType = ClassificationType.Dynamic, Name = "CurrentWeather", Description = "Current weather data for a specific location")]
    public class CurrentWeather
    {
        [OmfProperty(IsIndex = true)]
        public DateTime Timestamp { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int WeatherId { get; set; }
        public string WeatherMain { get; set; }
        public string WeatherDescription { get; set; }
        public string WeatherIcon { get; set; }
        public double Humidity { get; set; }
        public double Temp { get; set; }
        public double TempFeelsLike { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public double Pressure { get; set; }
        public double PressureSeaLevel { get; set; }
        public double PressureGroundLevel { get; set; }
        public double WindSpeed { get; set; }
        public double WindDeg { get; set; }
        public double CloudCover { get; set; }
        public double Rain1H { get; set; }
        public double Rain3H { get; set; }
        public double Snow1H { get; set; }
        public double Snow3H { get; set; }
    }
}
