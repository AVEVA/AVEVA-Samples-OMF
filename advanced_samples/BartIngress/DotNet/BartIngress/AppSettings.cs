using System;

namespace BartIngress
{
    /// <summary>
    /// Represents the application settings defined in appsettings.json
    /// </summary>
    public class AppSettings
    {
        #region BART API Settings

        /// <summary>
        /// BART API key from http://api.bart.gov/api/register.aspx
        /// </summary>
        public string BartApiKey { get; set; }

        /// <summary>
        /// Specifies the origin station abbreviation; "all" will get all origin ETDs
        /// </summary>
        public string BartApiOrig { get; set; }

        /// <summary>
        /// Specifies the destination station abbreviation; "all", will parse all destination ETDs
        /// </summary>
        public string BartApiDest { get; set; }

        #endregion

        #region OSIsoft Cloud Services Settings

        /// <summary>
        /// Specifies whether this application should send to OSIsoft Cloud Services
        /// </summary>
        public bool SendToOcs { get; set; }

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

        #region Edge Data Store Settings

        /// <summary>
        /// Specifies whether this application should send to the local Edge Data Store
        /// </summary>
        public bool SendToEds { get; set; }

        /// <summary>
        /// Edge Data Store Port, usually 5590
        /// </summary>
        public int EdsPort { get; set; }

        #endregion

        #region PI Web API Settings

        /// <summary>
        /// Specifies whether this application should send to a PI Web API OMF endpoint
        /// </summary>
        public bool SendToPi { get; set; }

        /// <summary>
        /// PI Web API Endpoint URI, like https://server//piwebapi
        /// </summary>
        public Uri PiWebApiUri { get; set; }

        /// <summary>
        /// Domain user name to use for Basic authentication against PI Web API
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Domain user password to use for Basic authentication against PI Web API
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Whether to validate the PI Web API endpoint certificate. Setting to false should only be done for testing with a self-signed PI Web API certificate as it is insecure. 
        /// </summary>
        public bool ValidateEndpointCertificate { get; set; }

        /// <summary>
        /// (Optional) Only used by test for verification purposes, the name of the PI Data Archive where OMF data is sent.
        /// </summary>
        public string TestPiDataArchive { get; set; }

        #endregion
    }
}
