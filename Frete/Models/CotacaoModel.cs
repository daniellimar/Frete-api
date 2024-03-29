﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Frete.Models
{
	public class CotacaoModel
	{
		public int Id { get; set; }
		public string SellerCEP { get; set; }
		public string RecipientCEP { get; set; }
		public int ShippingServiceCode { get; set; }

		[Column(TypeName = "decimal(10, 2)")]
		public decimal ShipmentInvoiceValue { get; set; }
		public string Width { get; set; }
		public string Length { get; set; }
		public string Height { get; set; }
		[Column(TypeName = "decimal(10, 3)")]
		public decimal Weight { get; set; }
		public int Quantity { get; set; }
		public string RecipientCountry { get; set; }
		public DateTime? DateLastUpdate { get; set; } = DateTime.Now;
	}
}
