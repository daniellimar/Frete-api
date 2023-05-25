using Frete.Data;
using Frete.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Web.Helpers;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

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

            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://api.frenet.com.br/shipping/info";
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    TempData["RespostaAPI"] = responseContent;
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
                var dados = JsonConvert.DeserializeObject<dynamic>(respostaAPI);
                var jsonArrayString = (JArray)dados.ShippingSeviceAvailableArray;
                List<Frete.Models.ShippingService> shippingServices = jsonArrayString.ToObject<List<Frete.Models.ShippingService>>();

                return View(shippingServices);
            }
            return RedirectToAction("Error");
        }

        public async Task<ActionResult> ConsultarFrete(FreteModel formulario)
        {
            if (formulario is null)
            {
                return BadRequest();
            }

            using (HttpClient client = new HttpClient())
            {
                // Define a URL da API
                string apiUrl = "https://api.frenet.com.br/shipping/quote";

                // Define o conteúdo da requisição (corpo) como um objeto JSON
                var requestData = new
                {
                    SellerCEP = formulario.CepOrigem,
                    RecipientCEP = formulario.CepDestino,
                    ShipmentInvoiceValue = formulario.ValorRemessa,
                    Quantity = formulario.Quantidade,
                    ShippingServiceCode = (string)null,
                    ShippingItemArray = new[]
                    {
                        new
                        {
                            Height = formulario.Altura,
                            Length = formulario.Comprimento,
                            Weight = formulario.Peso,
                            Width = formulario.Largura,
                        }
                    },
                    RecipientCountry = "BR"
                };

                // Serializa o objeto JSON para uma string
                string requestJson = JsonConvert.SerializeObject(requestData);

                // Cria o conteúdo da requisição HTTP com o corpo da requisição
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                // Define os headers da requisição
                content.Headers.Add("Token", "B9CDA873RC7ACR4864R9E36R03EFF0B7C4B7");
                content.Headers.Add("Chave", "daniel.engca@outlook.com");
                content.Headers.Add("senha", "A652T4gjQIJIFBrxwd4FQ==");

                // Envie a requisição POST para a API
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                // Verifique a resposta
                if (response.IsSuccessStatusCode)
                {
                    // A requisição foi bem-sucedida, você pode obter a resposta
                    string responseContent = await response.Content.ReadAsStringAsync();
                    TempData["RespostaAPIQUOTA"] = responseContent;
                    return RedirectToAction("CotacaoResponse");
                }
                return RedirectToAction("Error");

            }
        }
        public IActionResult CotacaoResponse()
        {
            var jsonString = TempData["RespostaAPIQUOTA"] as string;

            if (!string.IsNullOrEmpty(jsonString))
            {
                var shippingServiceResponse = JsonConvert.DeserializeObject<ShippingServiceResponse>(jsonString);
                var shippingServices = shippingServiceResponse?.ShippingSevicesArray;

                if (shippingServices != null)
                {
                    return View(shippingServices);
                }
            }
            return RedirectToAction("Error");
        }



        public IActionResult Error()
        {
            return View();
        }
    }
}