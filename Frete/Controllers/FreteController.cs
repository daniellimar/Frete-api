using Frete.Data;
using Frete.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frete.Controllers
{

	public class FreteController : Controller
	{
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

            FreteModel model = new FreteModel();
            model.CepOrigem = formulario.CepOrigem; 
            model.CepDestino = formulario.CepDestino; 
            model.CodigoServicoEnvio = formulario.CodigoServicoEnvio; 
            model.ValorRemessa = formulario.ValorRemessa;
            model.Largura = formulario.Largura;
            model.Comprimento = formulario.Comprimento;
            model.Altura = formulario.Altura;
            model.Peso = formulario.Peso;
            model.Quantidade = formulario.Quantidade;

            _db.Frete.Add(model);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}