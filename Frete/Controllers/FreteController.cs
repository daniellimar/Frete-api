using Microsoft.AspNetCore.Mvc;

namespace Frete.Controllers
{
	public class FreteController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
