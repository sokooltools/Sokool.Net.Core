using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Docs.Samples;
using Microsoft.Extensions.Logging;
using Sokool.Net.Web.Models;

namespace Sokool.Net.Web.Controllers
{
	public class SamplesController : Controller
	{
		[ViewData]
		// ReSharper disable once MemberCanBePrivate.Global
		public string Title { get; set; }
		
		private readonly ILogger<SamplesController> _logger;
		private readonly IWebHostEnvironment _env;

		public SamplesController(IWebHostEnvironment env, ILogger<SamplesController> logger)
		{
			_env = env;
			_logger = logger;
		}

		[HttpGet]
		public ActionResult Samples1()
		{
			Title = "Samples1";

			_logger.LogInformation(ControllerContext.ToCtxString());

			const string virtualFolder = "samples";
			const string extension = ".htm";
			string physicalFolder = Utils.CombinePaths(_env.WebRootPath, virtualFolder);
			return View(new SamplesModel(virtualFolder, physicalFolder, extension));
		}

		[HttpGet]
		public ActionResult Samples2()
		{
			Title = "Samples2";

			_logger.LogInformation(ControllerContext.ToCtxString());

			const string virtualFolder = "views/samples/";
			const string extension = ".cshtml";
			string physicalFolder = Utils.CombinePaths(_env.ContentRootPath, virtualFolder);
			return View(new SamplesModel(virtualFolder, physicalFolder, extension));
		}

		//[HttpGet]
		//public ActionResult ReadFile()
		//{
		//	return View();
		//}

		[HttpGet]
		public new ActionResult BadRequest() // 400
		{
			Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return new EmptyResult();
		}

		//[HttpGet]
		//public new ActionResult BadRequest() // 400
		//{
		//	//return BadRequest(null);
		//	return StatusCode((int)HttpStatusCode.BadRequest, "A bad request has been made.");
		//}

		[HttpGet]
		//public new ActionResult Unauthorized() // 401
		//{
		//	Response.StatusCode = (int)HttpStatusCode.Unauthorized;
		//	return new EmptyResult();
		//}

		[HttpGet]
		//[Route("/home/error")]
		public new ActionResult Unauthorized() // 401
		{
			return base.Unauthorized(null);
			//return StatusCode((int)HttpStatusCode.Unauthorized, "You are unauthorized.");
		}

		[HttpGet]
		public ActionResult RequestTimeout() // 408
		{
			Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
			return new EmptyResult();
		}

		[HttpGet]
		public ActionResult Unavailable() // 503
		{
			Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
			return new EmptyResult();
		}

		[HttpGet]
		public ActionResult NullReferenceException() // 500
		{
			string msg = null;
			// ReSharper disable once PossibleNullReferenceException
			ViewBag.Message = msg.Length; // This will throw an exception
			return new EmptyResult();
		}

		[HttpGet]
		//[HandleError(Order = 1, View = "DivideByZero", ExceptionType = typeof(DivideByZeroException))]
#pragma warning disable CA1822 // Mark members as static
		public ActionResult DivideByZeroException()
#pragma warning restore CA1822 // Mark members as static
		{
			int i = 10;
			// ReSharper disable once IntDivisionByZero
			i /= 0; // This will throw an exception
			Console.WriteLine(i);
			return new EmptyResult();
		}
	}
}