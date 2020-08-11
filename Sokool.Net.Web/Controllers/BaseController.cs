using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sokool.Net.Web.Controllers
{
	public abstract class BaseController : Controller
	{
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Called when an unhandled exception occurs in the action.
		/// </summary>
		/// <param name="filterContext">Information about the current request and action.</param>
		//------------------------------------------------------------------------------------------------------------------------
		protected override void OnException(ExceptionContext filterContext)
		{
			filterContext.ExceptionHandled = true;
			TempData["controller"] = filterContext.RouteData.Values["controller"].ToString();
			TempData["action"] = filterContext.RouteData.Values["action"].ToString();
			TempData["exception"] = filterContext.Exception;
			filterContext.Result = RedirectToAction("InternalException", "ErrorHandler"); //, new { exceptionType = e.GetType().Name } // this will add querystring to URL
		}

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Called when a request matches this controller, but no method with the specified action name is found in the 
		/// controller.
		/// </summary>
		/// <remarks>
		/// This gets called instead of the 'Application_Error' method inside Global.asax when it is not commented out.
		/// </remarks>
		/// <param name="actionName">The name of the attempted action.</param>
		//------------------------------------------------------------------------------------------------------------------------
		protected override void HandleUnknownAction(string actionName)
		{
			try
			{
				this.View(actionName).ExecuteResult(this.ControllerContext);
			}
			catch
			{
				Response.Redirect("~/ErrorHandler/httperror/404", true);
			}
		}
	}
}