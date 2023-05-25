namespace Frete.Models
{
    public class ShippingServiceItem
    {
        public string ServiceCode { get; set; }
        public string ServiceDescription { get; set; }
        public string Carrier { get; set; }
        public string CarrierCode { get; set; }
        public decimal ShippingPrice { get; set; }
        public int DeliveryTime { get; set; }
        public bool Error { get; set; }
        public int OriginalDeliveryTime { get; set; }
        public decimal OriginalShippingPrice { get; set; }
        public decimal ResponseTime { get; set; }
        public bool AllowBuyLabel { get; set; }
    }
}
