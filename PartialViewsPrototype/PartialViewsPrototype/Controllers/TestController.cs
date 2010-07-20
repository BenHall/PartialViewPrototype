using System.Web.Mvc;

namespace PartialViewsPrototype.Controllers
{
	public class TestController : Controller
	{
		public ActionResult Index() {
			int v = 1;
			if (TempData.ContainsKey("Test"))
				v = (int) TempData["Test"];
			return PartialView(v);
		}

		public ActionResult Submit(int test, string action, string controller) {
			TempData["Test"] = test*2;
			return RedirectToAction(action, controller, test * 2);
		}
	}
}