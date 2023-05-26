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
            IEnumerable<FreteModel> emprestimos = _db.Cotacoes;
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

        public async Task<ActionResult> ConsultarFrete(FreteModel formulario)
        {
            if (formulario is null)
            {
                return BadRequest();
            }

            //return Ok(formulario);

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
					//return Ok(responseContent);



					string connectionString = "server=DANIELLIMA\\SQLEXPRESS; Database=Frete; trusted_connection=true; TrustServerCertificate=True;"; // Replace with your actual connection string

					using (SqlConnection connection = new SqlConnection(connectionString))
					{
						connection.Open();

						// Parse the JSON response
						JObject responseObject = JObject.Parse(responseContent);

						// Get the arrays of items from the response
						JArray serviceCodeArray = responseObject["ServiceCode"] as JArray;
						JArray serviceDescriptionArray = responseObject["ServiceDescription"] as JArray;
						JArray carrierArray = responseObject["Carrier"] as JArray;
						JArray carrierCodeArray = responseObject["CarrierCode"] as JArray;
						JArray shippingPriceArray = responseObject["ShippingPrice"] as JArray;
						JArray deliveryTimeArray = responseObject["DeliveryTime"] as JArray;
						JArray errorArray = responseObject["Error"] as JArray;
						JArray originalDeliveryTimeArray = responseObject["OriginalDeliveryTime"] as JArray;
						JArray originalShippingPriceArray = responseObject["OriginalShippingPrice"] as JArray;
						JArray responseTimeArray = responseObject["ResponseTime"] as JArray;
						JArray allowBuyLabelArray = responseObject["AllowBuyLabel"] as JArray;

						if (serviceCodeArray != null && serviceDescriptionArray != null && carrierArray != null && carrierCodeArray != null &&
							shippingPriceArray != null && deliveryTimeArray != null && errorArray != null && originalDeliveryTimeArray != null &&
							originalShippingPriceArray != null && responseTimeArray != null && allowBuyLabelArray != null)
						{
							// Iterate over each item in the arrays
							for (int i = 0; i < serviceCodeArray.Count; i++)
							{
								using (SqlCommand command = new SqlCommand("InsertShippingService", connection))
								{
									command.CommandType = CommandType.StoredProcedure;

									// Extract values from the arrays
									string serviceCode = (string)serviceCodeArray[i];
									string serviceDescription = (string)serviceDescriptionArray[i];
									string carrier = (string)carrierArray[i];
									string carrierCode = (string)carrierCodeArray[i];
									decimal shippingPrice = (decimal)shippingPriceArray[i];
									int deliveryTime = (int)deliveryTimeArray[i];
									bool error = (bool)errorArray[i];
									string originalDeliveryTime = (string)originalDeliveryTimeArray[i];
									string originalShippingPrice = (string)originalShippingPriceArray[i];
									string responseTime = (string)responseTimeArray[i];
									bool allowBuyLabel = (bool)allowBuyLabelArray[i];

									// Add parameters
									command.Parameters.AddWithValue("@ServiceCode", serviceCode);
									command.Parameters.AddWithValue("@ServiceDescription", serviceDescription);
									command.Parameters.AddWithValue("@Carrier", carrier);
									command.Parameters.AddWithValue("@CarrierCode", carrierCode);
									command.Parameters.AddWithValue("@ShippingPrice", shippingPrice);
									command.Parameters.AddWithValue("@DeliveryTime", deliveryTime);
									command.Parameters.AddWithValue("@Error", error);
									command.Parameters.AddWithValue("@Msg", string.Empty); // Since there's no "Msg" property in the JSON structure
									command.Parameters.AddWithValue("@OriginalDeliveryTime", originalDeliveryTime);
									command.Parameters.AddWithValue("@OriginalShippingPrice", originalShippingPrice);
									command.Parameters.AddWithValue("@ResponseTime", responseTime);
									command.Parameters.AddWithValue("@AllowBuyLabel", allowBuyLabel);

									// Execute the command
									object result = command.ExecuteScalar();

									// Check the result
									if (result != null && result != DBNull.Value)
									{
										int shippingServiceID = Convert.ToInt32(result);
										// Handle the inserted ShippingServiceID as needed
									}
								}
							}
						}
						else
						{
							// Handle the case when any of the arrays are not found in the JSON response
						}
					}



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