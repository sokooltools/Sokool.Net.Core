using System.Web.Mvc;
using Sokool.Net.DataLibrary.BusinessLogic;
using Sokool.Net.Web.Models;

namespace Sokool.Net.Web.Controllers
{
	//[CustomActionFilter("HomeController")]
	public class HomeController : BaseController
	{
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		//[CustomActionFilter("About")]
		public ActionResult About()
		{
			ViewBag.Message = "About the Sokool.net web-site";
			return View();
		}

		[HttpGet]
		public ActionResult Contact()
		{
			ViewBag.Message = "How to contact Sokool.net web-site support.";
			return View();
		}

		[HttpGet]
		public ActionResult SignUp()
		{
			ViewBag.Message = "Sign-Up for full access to Sokool.net.";
			return View(new UserModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SignUp(UserModel model)
		{
			if (!ModelState.IsValid)
				return View();

			UserProcessor.CreateUser(model.UserId, model.FirstName, model.LastName, model.EmailAddress);
			return RedirectToAction("Index");
		}

	}
}