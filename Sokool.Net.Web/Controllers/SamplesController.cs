using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Docs.Samples;
using Microsoft.Extensions.Logging;
using Sokool.Net.Web.Models;

namespace Sokool.Net.Web.Controllers
{
	[Authorize(Roles = "SuperUser, Admin")]
	public class SamplesController : Controller
	{
		[ViewData]
		// ReSharper disable once MemberCanBePrivate.Global
		public string Title { get; set; }
		
		private readonly ILogger<SamplesController> _logger;
		private readonly IWebHostEnvironment _env;

		public SamplesController(ILogger<SamplesController> logger, IWebHostEnvironment env)
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
			return View(new SamplesViewModel(virtualFolder, physicalFolder, extension));
		}

		[HttpGet]
		public ActionResult Samples2()
		{
			Title = "Samples2";
			_logger.LogInformation(ControllerContext.ToCtxString());

			const string virtualFolder = "views/samples/";
			const string extension = ".cshtml";
			string physicalFolder = Utils.CombinePaths(_env.ContentRootPath, virtualFolder);
			return View(new SamplesViewModel(virtualFolder, physicalFolder, extension));
		}

		[HttpGet]
		[Route("/Samples/Test")]
		public ActionResult Test(int statusCode)
		{
			Response.StatusCode = statusCode;
			return new EmptyResult();
		}

		[HttpGet]
		public ActionResult DivideByZeroException()
		{
			throw new DivideByZeroException();
		}

		[HttpGet]
		public ActionResult NullReferenceException()
		{
			throw new NullReferenceException();
		}

		//[HttpGet]
		//public ActionResult NullReferenceException()
		//{
		//	string msg = null;
		//	ViewBag.Message = msg.Length; // This will throw an exception
		//	return new EmptyResult();
		//}

		//[HttpGet]
		//public ActionResult DivideByZeroException()
		//{
		//	int i = 1;
		//	i /= 0; // This will throw an exception
		//	return new EmptyResult();
		//}
	}
}