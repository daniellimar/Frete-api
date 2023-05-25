using Frete.Data;
using Frete.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;


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
        public async Task<ActionResult> Salvar(FreteModel formulario)
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

            // Consultar a API externa
            using (HttpClient client = new HttpClient())
            {
                // Fazer a requisição GET para a URL da API externa
                string apiUrl = "https://api.frenet.com.br/shipping/info";
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Ler a resposta da API
                    string responseContent = await response.Content.ReadAsStringAsync();
                    // Armazenar a resposta na TempData
                    TempData["RespostaAPI"] = responseContent;

                    // Processar a resposta como necessário
                    // ...
                }
                else
                {
                    // Lidar com o caso em que a requisição não foi bem-sucedida
                    // ...
                }
            }
            return RedirectToAction("Cotacao");
        }
        public IActionResult Cotacao()
        {
            string respostaAPI = TempData["RespostaAPI"] as string;

            if (!string.IsNullOrEmpty(respostaAPI))
            {
                // Converter a respostaAPI em objetos C# usando a desserialização JSON
                var dados = JsonConvert.DeserializeObject<dynamic>(respostaAPI);

                // Obter a propriedade ShippingSeviceAvailableArray como um JArray
                var jsonArrayString = (JArray)dados.ShippingSeviceAvailableArray;

                // Converter o JArray em List<ShippingService>
                List<Frete.Models.ShippingService> shippingServices = jsonArrayString.ToObject<List<Frete.Models.ShippingService>>();

                // Passar a lista para a View
                return View(shippingServices);


                // Passar a string jsonArrayString para a view ou fazer o que desejar com ela
                return View(jsonArrayString);
            }
            else
            {
                // Lidar com o caso em que a respostaAPI é nula ou vazia
                // Por exemplo, redirecionar para uma página de erro ou retornar uma mensagem de erro
                return RedirectToAction("Erro");
            }
        }
    }
}