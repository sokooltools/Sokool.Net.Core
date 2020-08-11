using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Sokool.Net.Web.Controllers
{
	public class ErrorHandlerController : BaseController
	{

		[HttpGet]
		public ActionResult InternalException() // 500
		{
			Exception ex = TempData.ContainsKey("Exception") ? TempData["Exception"] as Exception : new HttpException(500, "Unknown Internal Exception");
			// Log the exception
			Trace.TraceError($"{DateTime.Now} exception={ex?.GetType().Name}, message={ex?.Message}");
			return View();
		}

		[HttpGet]
		public ActionResult HttpError(string id) // 
		{
			int iid = Int32.Parse(id);
			TempData["errorcode"] = iid;
			TempData["errordesc1"] = _lookup[iid].Desc1;
			TempData["errordesc2"] = _lookup[iid].Desc2;
			// Log the exception
			Trace.TraceError($"{DateTime.Now} errorcode={TempData["errorcode"]}, {TempData["errordesc1"]}, {TempData["errordesc2"]}");
			return View();
		}

		private class Lkp
		{
			public readonly string Desc1;
			public readonly string Desc2;

			public Lkp(string desc1, string desc2)
			{
				Desc1 = desc1;
				Desc2 = desc2;
			}
		}

		private readonly Dictionary<int, Lkp> _lookup = new Dictionary<int, Lkp>()
		{
			{400, new Lkp("BadRequest", "A bad request was made")},
			{401, new Lkp("Unauthorized", "Access is denied due to invalid credentials")},
			{403, new Lkp("Forbidden: Access is denied", "Access to the requested resource is forbidden")},
			{404, new Lkp("File or directory not found", "The resource you requested may have been removed, had its name changed, or is temporarily unavailable.")},
			{405, new Lkp("HTTP verb used to access this page not allowed", "The page you are looking for cannot be displayed because an invalid method (HTTP verb) was used to attempt access.")},
			{406, new Lkp("Client browser does not accept the MIME type of the requested page", "The page you are looking for cannot be opened by your browser because it has a file name extension that your browser does not accept.")},
			{408, new Lkp("RequestTimeout", "The request timed out")},
			{500, new Lkp("Internal server error", "There is a problem with the resource you are looking for, and it cannot be displayed")},
			{503, new Lkp("Service Unavailable", "The server cannot handle the request (because it is overloaded or down for maintenance)")}
		};
	}
}