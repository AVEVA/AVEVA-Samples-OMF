using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWeather
{
    public class AppSettings
    {
        #region OpenWeather API

        /// <summary>
        /// OpenWeather API Endpoint URI
        /// </summary>
        public Uri OpenWeatherUri { get; set; }

        /// <summary>
        /// OpenWeather API Key
        /// </summary>
        public string OpenWeatherKey { get; set; }

        /// <summary>
        /// OpenWeather API Queries, | separated
        /// </summary>
        public string OpenWeatherQueries { get; set; }
        #endregion

        #region OSIsoft Cloud Services

        /// <summary>
        /// OSIsoft Cloud Services OMF Endpoint URI
        /// </summary>
        public Uri OcsUri { get; set; }

        /// <summary>
        /// OSIsoft Cloud Services Tenant ID
        /// </summary>
        public string OcsTenantId { get; set; }

        /// <summary>
        /// OSIsoft Cloud Services Namespace ID
        /// </summary>
        public string OcsNamespaceId { get; set; }

        /// <summary>
        /// OSIsoft Cloud Services Client ID
        /// </summary>
        public string OcsClientId { get; set; }

        /// <summary>
        /// OSIsoft Cloud Services Client Secret
        /// </summary>
        public string OcsClientSecret { get; set; }
        #endregion
    }
}
