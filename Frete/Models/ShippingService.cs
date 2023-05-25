namespace Frete.Models
{
    public class ShippingService
    {
        public string ServiceCode { get; set; }
        public string ServiceDescription { get; set; }
        public string Carrier { get; set; }
        public string CarrierCode { get; set; }
        public string ShippingPrice { get; set; }
        public string DeliveryTime { get; set; }
        public bool Error { get; set; }
        public string Msg { get; set; }
        public string OriginalDeliveryTime { get; set; }
        public string OriginalShippingPrice { get; set; }
        public string ResponseTime { get; set; }
        public bool AllowBuyLabel { get; set; }
    }
    class ShippingServiceResponse
    {
        public List<ShippingService> ShippingSevicesArray { get; set; }
    }
}
