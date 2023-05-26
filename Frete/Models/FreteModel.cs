using System.Data;

namespace Frete.Models
{
	public class FreteModel
	{
		public int Id { get; set; }
		public string SellerCEP { get; set; }
		public string RecipientCEP { get; set; }
		public string ShippingServiceCode { get; set; }
		public decimal ShipmentInvoiceValue { get; set; }
		public decimal Width { get; set; }
		public decimal Length { get; set; }
		public decimal Height { get; set; }
		public decimal Weight { get; set; }
		public int Quantity { get; set; }
		public string RecipientCountry { get; set; }
		public bool? Status { get; set; }
		public DateTime? DateLastUpdate { get; set; } = DateTime.Now;
	}
}
