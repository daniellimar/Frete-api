using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frete.Models
{
    public class ShippingServiceModel
	{
		public int Id { get; set; }
		public string ServiceCode { get; set; }
		public string ServiceDescription { get; set; }
        public string Carrier { get; set; }
        public string CarrierCode { get; set; }

		[Column(TypeName = "decimal(10, 2)")]
		public decimal ShippingPrice { get; set; }
        public string DeliveryTime { get; set; }
        public bool Error { get; set; }
        public string Msg { get; set; }
        public string OriginalDeliveryTime { get; set; }

		[Column(TypeName = "decimal(10, 2)")]
		public decimal OriginalShippingPrice { get; set; }
        public string ResponseTime { get; set; }
        public bool AllowBuyLabel { get; set; }
    }
    class ShippingServiceResponse
    {
        public List<ShippingServiceModel> ShippingSevicesArray { get; set; }
    }
}
