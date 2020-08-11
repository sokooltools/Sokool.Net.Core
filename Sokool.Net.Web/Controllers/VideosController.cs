using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Docs.Samples;
using Microsoft.Extensions.Logging;
using Sokool.Net.DataLibrary.Data;
using Sokool.Net.Web.Models;

namespace Sokool.Net.Web.Controllers
{
	public class VideosController : Controller
	{
		// ReSharper disable once MemberCanBePrivate.Global
		[ViewData] public string Title { get; set; }

		private readonly ILogger<VideosController> _logger;
		private readonly IWebHostEnvironment _env;

		public VideosController(IWebHostEnvironment env, ILogger<VideosController> logger)
		{
			_env = env;
			_logger = logger;
		}

		[HttpGet]
		[Route("[controller]/[action]/{vf?}",  Name = "[controller]_[action]")]
		public ActionResult Index(string vf = "Across") // , string ext = ".mp4"
		{
			Title = "Video Gallery";

			_logger.LogInformation(ControllerContext.ToCtxString());

			string virtualFolderPath = $@"/assets/videos/{vf}/";

			string physicalFolderPath = Utils.CombinePaths(_env.ContentRootPath, virtualFolderPath);
			if (!Directory.Exists(physicalFolderPath))
				throw new InvalidOperationException("The specified directory does not exist.");

			return View(new VideosModel(vf, physicalFolderPath, ".mp4").Sort());
		}

		[HttpGet]
		[Route("[controller]/[action]/{vf?}",  Name = "[controller]_[action]")]
		public ActionResult Show(string vf = "Across", string fn = "Hey Jude.mp4")
		{
			Title = "Video";

			_logger.LogInformation(ControllerContext.ToCtxString());

			string virtualFolderPath = $@"/assets/videos/{vf}/";

			string physicalFilePath = Utils.CombinePaths(_env.ContentRootPath, virtualFolderPath, fn);
			if (!System.IO.File.Exists(physicalFilePath))
				throw new InvalidOperationException("The specified file does not exist.");

			return View(new Video(virtualFolderPath, physicalFilePath));
		}
	}
}