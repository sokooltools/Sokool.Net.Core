using System;
using System.Collections.Concurrent;
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

		[ViewData] private static string VirtualFolder { get; set; }
		[ViewData] private static string SortColumn { get; set; }
		[ViewData] private static string SortDirection { get; set; }

		private readonly ILogger<VideosController> _logger;
		private readonly IWebHostEnvironment _env;

		public VideosController(IWebHostEnvironment env, ILogger<VideosController> logger)
		{
			_env = env;
			_logger = logger;
		}

		private static readonly ConcurrentDictionary<string, IEnumerable<Video>> Cache = new ConcurrentDictionary<string, IEnumerable<Video>>();

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Indexes the specified vf.
		/// </summary>
		/// <param name="vf">The name of the folder containing the categorized videos.</param>
		/// <param name="sortColumn">
		/// Column name to sort the results by; [Default is the 'Name' column].
		/// </param>
		/// <param name="sortDirection">
		/// Direction to sort the results by; [Default is "asc"ending order].
		/// </param>
		/// <param name="nameStartsWith">
		/// A string when provided, limits the videos being displayed to those whose name 'starts with'; By default all videos are 
		/// returned.
		/// </param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException">The specified list of videos does not exist.</exception>
		//------------------------------------------------------------------------------------------------------------------------
		[HttpGet]
		[Route("[controller]/[action]/{vf?}", Name = "[controller]_[action]")]
		public ActionResult Index(string vf = null, string sortColumn = null, string sortDirection = null, string nameStartsWith = null) // , string ext = ".mp4"
		{
			Title = "Video Gallery";

			_logger.LogInformation(ControllerContext.ToCtxString());

			IEnumerable<Video> videos;

			bool isSameVf = vf == VirtualFolder;

			vf ??= VirtualFolder ??= "across";

			if (Cache.ContainsKey(vf))
			{
				videos = Cache[vf].ToList();
			}
			else
			{
				string virtualFolderPath = $@"/assets/videos/{vf}/";

				string physicalFolderPath = Utils.CombinePaths(_env.ContentRootPath, virtualFolderPath);
				if (!Directory.Exists(physicalFolderPath))
					throw new InvalidOperationException("The specified list of videos does not exist.");

				videos = new VideosViewModel(vf, physicalFolderPath, ".mp4").ToList();

				Cache.TryAdd(vf, videos);
			}

			sortColumn ??= SortColumn ??= "name";

			switch (sortColumn)
			{
				case "name":
				{
					if (isSameVf && SortColumn == "name")
						// Toggle the direction.
						sortDirection = sortDirection == null && SortDirection == "asc" ? "desc" : "asc"; 
					else 
						sortDirection = SortDirection ?? "asc";
					videos = sortDirection == "asc" ? videos.OrderBy(m => m.Name) : videos.OrderByDescending(m => m.Name);
					break;
				}
				case "length":
				{
					if (isSameVf && SortColumn == "length")
						sortDirection = sortDirection == null &&  SortDirection == "asc" ? "desc" : "asc";
					else 
						sortDirection = SortDirection;
					videos = sortDirection == "asc" ? videos.OrderBy(m => m.Length) : videos.OrderByDescending(m => m.Length);
					break;
				}
			}

			VirtualFolder = vf;
			SortColumn = sortColumn;
			SortDirection = sortDirection;

			if (nameStartsWith != null)
				videos = videos.Where(m => m.Name.StartsWith(nameStartsWith));

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