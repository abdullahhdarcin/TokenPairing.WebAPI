namespace TokenPairing.Web.Models
{
    public class ServiceApiSettings
    {
        public string GatewayBaseUri { get; set; }
        public string MemberUri { get; set; }
        
        public ServiceApi Member { get; set; }

        public class ServiceApi
        {
            public string Path { get; set; }
        }
    }
}
