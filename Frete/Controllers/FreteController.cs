using Frete.Data;
using Frete.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;

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
            IEnumerable<FreteModel> emprestimos = _db.Cotacoes;
            var gruposPorCepOrigem = emprestimos.GroupBy(e => e.SellerCEP)
                                    .Select(g => new { SellerCEP = g.Key, CepDestino = g.Select(e => e.RecipientCEP), Quantity = g.Count() });

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

                    command.Parameters.AddWithValue("@CepOrigem", formulario.SellerCEP);
                    command.Parameters.AddWithValue("@CepDestino", formulario.RecipientCEP);
                    command.Parameters.AddWithValue("@CodigoServicoEnvio", formulario.ShippingServiceCode);
                    command.Parameters.AddWithValue("@ValorRemessa", formulario.ShipmentInvoiceValue);
                    command.Parameters.AddWithValue("@Largura", formulario.Width);
                    command.Parameters.AddWithValue("@Comprimento", formulario.Length);
                    command.Parameters.AddWithValue("@Altura", formulario.Height);
                    command.Parameters.AddWithValue("@Peso", formulario.Weight);
                    command.Parameters.AddWithValue("@Quantidade", formulario.Quantity);

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
                    return RedirectToAction("Cotacao");
                }
                return RedirectToAction("Error");
            }
        }
        public IActionResult Cotacao()
        {
            string respostaAPI = TempData["RespostaAPI"] as string;

            if (!string.IsNullOrEmpty(respostaAPI))
            {
                var dados = JsonConvert.DeserializeObject<dynamic>(respostaAPI);
                var jsonArrayString = (JArray)dados.ShippingSeviceAvailableArray;
                List<ShippingService> shippingServices = jsonArrayString.ToObject<List<Frete.Models.ShippingService>>();

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

			using (var connection = new SqlConnection("server=DANIELLIMA\\SQLEXPRESS; Database=Frete; trusted_connection=true; TrustServerCertificate=True;"))
			{
				using (var command = new SqlCommand("dbo.InsertCotacao", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					command.Parameters.AddWithValue("@SellerCEP", formulario.SellerCEP);
					command.Parameters.AddWithValue("@RecipientCEP", formulario.RecipientCEP);
					command.Parameters.AddWithValue("@ShippingServiceCode", formulario.ShippingServiceCode);
					command.Parameters.AddWithValue("@ShipmentInvoiceValue", formulario.ShipmentInvoiceValue);
					command.Parameters.AddWithValue("@Width", formulario.Width);
					command.Parameters.AddWithValue("@Length", formulario.Length);
					command.Parameters.AddWithValue("@Height", formulario.Height);
					command.Parameters.AddWithValue("@Weight", formulario.Weight);
					command.Parameters.AddWithValue("@Quantity", formulario.Quantity);
					command.Parameters.AddWithValue("@RecipientCountry", formulario.RecipientCountry);

				    connection.Open();
				    command.ExecuteNonQuery();
			    }
			}

			using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://api.frenet.com.br/shipping/quote";

                var requestData = new
                {
                    SellerCEP = formulario.SellerCEP,
                    RecipientCEP = formulario.RecipientCEP,
                    ShipmentInvoiceValue = formulario.ShipmentInvoiceValue,
                    Quantity = formulario.Quantity,
                    ShippingServiceCode = (string)null,
                    ShippingItemArray = new[]
                    {
                        new
                        {
                            Height = formulario.Width,
                            Length = formulario.Length,
                            Weight = formulario.Weight,
                            Width = formulario.Width,
                        }
                    },
                    RecipientCountry = formulario.RecipientCountry
				};

				string requestJson = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                
                content.Headers.Add("Token", "B9CDA873RC7ACR4864R9E36R03EFF0B7C4B7");
                content.Headers.Add("Chave", "daniel.engca@outlook.com");
                content.Headers.Add("senha", "A652T4gjQIJIFBrxwd4FQ==");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
				if (response.IsSuccessStatusCode)
                {
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