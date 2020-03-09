namespace BARTIngress
{
    class AppSettings
    {
        public string BartApiKey { get; set; }
        public string BartApiOrig { get; set; }
        public string BartApiDest { get; set; }

        public bool SendToOcs { get; set; }
        public string OcsUri { get; set; }
        public string OcsTenantId { get; set; }
        public string OcsNamespaceId { get; set; }
        public string OcsClientId { get; set; }
        public string OcsClientSecret { get; set; }

        public bool SendToEds { get; set; }
        public int EdsPort { get; set; }
    }
}
