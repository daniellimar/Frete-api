using System.Data;

namespace Frete.Models
{
    public class FreteModel
	{
        public int Id { get; set; }
        public string Recebedor { get; set; }
		public string Fornecedor { get; set; }
        public string LivroEmprestado { get; set; }
        public DateTime dataUltimaAtualizacao { get; set; } = DateTime.Now;
    }
}
