using Frete.Data;
using Frete.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;



namespace Frete.Controllers
{

	public class FreteController : Controller
	{
        private const string ConnectionString = "DefaultConnection";
        readonly private ApplicationDbContext _db;
		public FreteController(ApplicationDbContext db)
		{
			_db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<FreteModel> emprestimos = _db.Frete;
            var gruposPorCepOrigem = emprestimos.GroupBy(e => e.CepOrigem)
                                    .Select(g => new { CepOrigem = g.Key, CepDestino = g.Select(e => e.CepDestino), Quantidade = g.Count() });

            return View(gruposPorCepOrigem);
        }

        [HttpPost]
        public ActionResult Salvar(FreteModel formulario)
        {
            if (formulario is null)
            {
                return BadRequest();
            }

            using (var connection = new SqlConnection("server=DANIELLIMA\\SQLEXPRESS; Database=Frete; trusted_connection=true; TrustServerCertificate=True;"))
            {
                using (var command = new SqlCommand("dbo.AdicionarFrete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CepOrigem", formulario.CepOrigem);
                    command.Parameters.AddWithValue("@CepDestino", formulario.CepDestino);
                    command.Parameters.AddWithValue("@CodigoServicoEnvio", formulario.CodigoServicoEnvio);
                    command.Parameters.AddWithValue("@ValorRemessa", formulario.ValorRemessa);
                    command.Parameters.AddWithValue("@Largura", formulario.Largura);
                    command.Parameters.AddWithValue("@Comprimento", formulario.Comprimento);
                    command.Parameters.AddWithValue("@Altura", formulario.Altura);
                    command.Parameters.AddWithValue("@Peso", formulario.Peso);
                    command.Parameters.AddWithValue("@Quantidade", formulario.Quantidade);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }
    }
}