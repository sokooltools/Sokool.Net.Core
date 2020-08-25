using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Docs.Samples;
using Sokool.Net.DataLibrary.Data;
using Sokool.Net.Web.Models;

namespace Sokool.Net.Web.Controllers
{
	[Authorize]
	public class PhotosController : Controller
	{
		[ViewData]
		// ReSharper disable once MemberCanBePrivate.Global
		public string Title { get; set; }

		private readonly ILogger<PhotosController> _logger;
		private readonly IWebHostEnvironment _env;

		public PhotosController(IWebHostEnvironment env, ILogger<PhotosController> logger)
		{
			_env = env;
			_logger = logger;
		}

		[HttpGet]
		[Route("[controller]/[action]/{id:int?}", Name = "[controller]_[action]")]
		public ActionResult Photos1(int? id = 0)
		{
			Title = "Photos1";
			_logger.LogInformation(ControllerContext.ToCtxString());

			return View("Photos1", GetPhotosModel(id));
		}

		[HttpGet]
		[Route("[controller]/[action]/{id:int?}", Name = "[controller]_[action]")]
		public ActionResult Photos2(int? id = 0)
		{
			Title = "Photos2";
			return View("Photos2", GetPhotosModel(id));
		}

		private IOrderedEnumerable<Photo> GetPhotosModel(int? id)
		{
			string virtualFolder = "/assets/photos/";
			switch (id)
			{
				case 0:
					virtualFolder += "default/";
					break;
				case 1:
					virtualFolder += "cameras/";
					break;
				case 2:
					virtualFolder += "cars/";
					break;
				case 3:
					virtualFolder += "ships/";
					break;
				case 4:
					virtualFolder += "021310/";
					break;
			}
			string physicalFolder = Utils.CombinePaths(_env.ContentRootPath, virtualFolder);

			return new PhotosViewModel(virtualFolder, physicalFolder, ".jpg").ToList().OrderBy(m => m.Path);
		}
	}
}