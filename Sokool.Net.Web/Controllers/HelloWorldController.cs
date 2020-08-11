 using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Sokool.Net.Web.Controllers
{
	public class HelloWorldController : Controller
    {
		// GET: /HelloWorld or HelloWorld/Index
#pragma warning disable CA1822 // Mark members as static
		public string Index()
#pragma warning restore CA1822 // Mark members as static
		{
            return "This is my <b>default</b> action...";;
        }

		// GET:  /HelloWorld/Welcome/Scott/4
#pragma warning disable CA1822 // Mark members as static
		public string Welcome(string name, int id = 1)
#pragma warning restore CA1822 // Mark members as static
		{ 
	        return HttpUtility.HtmlEncode("Hello " + name + ", NumTimes is: " + id);
        } 
    }
}