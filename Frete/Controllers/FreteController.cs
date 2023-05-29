using Frete.Data;
using Frete.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using System.Threading;

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
            IEnumerable<CotacaoModel> emprestimos = _db.Cotacao;
            return View();
        }
        public IActionResult Cotacao()
		{
			string respostaAPI = TempData["RespostaAPI"] as string;

			if (!string.IsNullOrEmpty(respostaAPI))
            {
                var dados = JsonConvert.DeserializeObject<dynamic>(respostaAPI);
                var jsonArrayString = (JArray)dados.ShippingSeviceAvailableArray;
                List<ShippingServiceModel> shippingServices = jsonArrayString.ToObject<List<Frete.Models.ShippingServiceModel>>();

                return View(shippingServices);
            }
            return RedirectToAction("Error");
        }

        public async Task<ActionResult> ConsultarFrete(CotacaoModel formulario)
        {
            if (formulario is null)
            {
                return BadRequest();
            }

			using (var connection = new SqlConnection("server=DANIELLIMA\\SQLEXPRESS; Database=Frete; trusted_connection=true; TrustServerCertificate=True;"))
			{
				connection.Open();
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
					command.Parameters.AddWithValue("@DateLastUpdate", DateTime.Now);

					var insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int);
					insertedIdParam.Direction = ParameterDirection.Output;
					command.Parameters.Add(insertedIdParam);
					command.ExecuteNonQuery();

					var IdCotacao = (int)insertedIdParam.Value;
					TempData["IdCotacao"] = IdCotacao;
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

					if (!string.IsNullOrEmpty(responseContent))
					{
						var responseObj = JsonConvert.DeserializeObject<ShippingServiceResponse>(responseContent);

						if (responseObj != null && responseObj.ShippingSevicesArray != null)
						{
							foreach (var shippingService in responseObj.ShippingSevicesArray)
							{
                                //return Ok(shippingService);
								InsertShippingService(shippingService);
							}
						}
					}
					return RedirectToAction("CotacaoResponse");
                }
                return RedirectToAction("Error");
            }
		}
		private void InsertShippingService(ShippingServiceModel shippingService)
		{
			string connectionString = "server=DANIELLIMA\\SQLEXPRESS; Database=Frete; trusted_connection=true; TrustServerCertificate=True;"; // Replace with your actual connection string
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();

				using (SqlCommand command = new SqlCommand("InsertShippingService", connection))
				{
					int idCotacao = (int)TempData["IdCotacao"];

					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@CotacaoId", idCotacao);
					command.Parameters.AddWithValue("@ServiceCode", shippingService.ServiceCode);
					command.Parameters.AddWithValue("@ServiceDescription", shippingService.ServiceDescription);
					command.Parameters.AddWithValue("@Carrier", shippingService.Carrier);
					command.Parameters.AddWithValue("@CarrierCode", shippingService.CarrierCode);
					command.Parameters.AddWithValue("@ShippingPrice", shippingService.ShippingPrice);
					command.Parameters.AddWithValue("@DeliveryTime", shippingService.DeliveryTime == null ? 0 : shippingService.DeliveryTime);
					command.Parameters.AddWithValue("@Error", shippingService.Error);
                    command.Parameters.AddWithValue("@Msg", shippingService.Msg == null ? 0 : shippingService.Msg);
					command.Parameters.AddWithValue("@OriginalDeliveryTime", shippingService.OriginalDeliveryTime == null ? "0" : shippingService.OriginalDeliveryTime);
					command.Parameters.AddWithValue("@OriginalShippingPrice", shippingService.OriginalShippingPrice);
					command.Parameters.AddWithValue("@ResponseTime", shippingService.ResponseTime);
					command.Parameters.AddWithValue("@AllowBuyLabel", shippingService.AllowBuyLabel);

					var insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int);
					insertedIdParam.Direction = ParameterDirection.Output;
					command.Parameters.Add(insertedIdParam);
					command.ExecuteNonQuery();
				}
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