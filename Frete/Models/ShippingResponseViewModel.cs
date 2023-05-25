using Newtonsoft.Json.Linq;

namespace Frete.Models
{
    public class ShippingResponseViewModel
    {
        public List<ShippingServiceItem> ShippingServicesArray { get; set; }
        public int Timeout { get; set; }
        public JArray ShippingSevicesArray { get; internal set; }
    }

}
