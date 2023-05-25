using System.Data;

namespace Frete.Models
{
	public class FreteModel
	{
		public int Id { get; set; }
		public string CepOrigem { get; set; }
		public string CepDestino { get; set; }
		public string CodigoServicoEnvio { get; set; }
		public decimal ValorRemessa { get; set; }
		public decimal Largura { get; set; }
		public decimal Comprimento { get; set; }
		public decimal Altura { get; set; }
		public decimal Peso { get; set; }
		public int Quantidade { get; set; }
		public bool? Status { get; set; }
		public DateTime? DataUltimaAtualizacao { get; set; } = DateTime.Now;
	}
}
