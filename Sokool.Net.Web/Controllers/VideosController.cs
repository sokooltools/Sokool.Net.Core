using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
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
		//------------------------------------------------------------------------------------------------------------------------
		// ReSharper disable once MemberCanBePrivate.Global
		[ViewData] public string Title { get; set; }

		private readonly ILogger<VideosController> _logger;
		private readonly IWebHostEnvironment _env;

		public VideosController(IWebHostEnvironment env, ILogger<VideosController> logger)
		{
			_env = env;
			_logger = logger;
		}

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Indexes the specified vf.
		/// </summary>
		/// <param name="vf">The name of the folder containing the categorized videos.</param>
		/// <param name="sbs">
		/// A composite string containing the name of the column and the direction to sort the results by; By default the list of 
		/// videos are displayed using the 'Name' column in 'ascending' order.
		/// </param>
		/// <param name="sws">
		/// A string limiting the videos being displayed to those whose name 'starts with'; By default all videos are returned.
		/// </param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException">The specified list of videos does not exist.</exception>
		//------------------------------------------------------------------------------------------------------------------------
		[HttpGet]
		[Route("[controller]/[action]/{vf?}", Name = "[controller]_[action]")]
		public ActionResult Index(string vf = "across", string sbs = "name asc", string sws = null) // , string ext = ".mp4"
		{
			Title = "Video Gallery";

			_logger.LogInformation(ControllerContext.ToCtxString());

			string virtualFolderPath = $@"/assets/videos/{vf}/";

			string physicalFolderPath = Utils.CombinePaths(_env.ContentRootPath, virtualFolderPath);
			if (!Directory.Exists(physicalFolderPath))
				throw new InvalidOperationException("The specified list of videos does not exist.");

			IEnumerable<Video> videos = new VideosViewModel(vf, physicalFolderPath, ".mp4").Sort(sbs);

			if (sws != null) 
				videos = videos.Where(m => m.Name.StartsWith(sws));
			
			return View(videos);
		}

		[HttpGet]
		[Route("[controller]/[action]/{vf?}", Name = "[controller]_[action]")]
		public ActionResult Show(string vf = "across", string fn = "Hey Jude.mp4")
		{
			Title = "Video";

			_logger.LogInformation(ControllerContext.ToCtxString());

			string virtualFolderPath = $@"/assets/videos/{vf}/";

			string physicalFilePath = Utils.CombinePaths(_env.ContentRootPath, virtualFolderPath, fn);
			if (!System.IO.File.Exists(physicalFilePath))
				throw new InvalidOperationException("The specified video file does not exist.");

			var video = new Video(virtualFolderPath, physicalFilePath);
			return View(video);
		}
	}
}