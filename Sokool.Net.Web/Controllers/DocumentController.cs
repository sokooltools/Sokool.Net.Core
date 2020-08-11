using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Web.Hosting;
using System.IO;
using System.Net;
using System.Data.SqlClient;

namespace WebApplication4.Controllers
{
	public class DocumentController : Controller
	{
		PMPDIEntities db = new PMPDIEntities();

		// CreateList
		public ActionResult List(string document)
		{
			//var docum = db.Documents.Include(p => p.Project);
			var a = from p in db.Documents.Include(p => p.Project)
				select p;
			if (!String.IsNullOrEmpty(document))
			{
				a = a.Where(
					c => c.FileDName.Contains(document)
					     || c.Project.nameProject.Contains(document)
					     || c.TypeDocument.typeDocumentName.Contains(document)
				);
			}

			return View(a.ToList());
		}

		// CreateInsert
		public ActionResult Submit()
		{
			ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "nameProject");
			ViewBag.typeDocumentID = new SelectList(db.TypeDocuments, "typeDocumentID", "typeDocumentName");
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult submit(Document t, HttpPostedFileBase file,
			[Bind(Include = "DocumentID, Doc, FileDName, typeDocumentID, ProjectID, FileContent, ContentType")]
			Document document)
		{
			if (file != null && file.ContentLength > 0)
			{
				string ds = file.FileName.Substring(file.FileName.Length - 3);
				string p = string.Empty;
				p = Server.MapPath("~/Files/");
				file.SaveAs(p + file.FileName);
				if (file.ContentLength > 0)
				{
					var br = new BinaryReader(file.InputStream);
					byte[] buffer = br.ReadBytes(file.ContentLength);
					using (db)
					{
						t.Doc = file.FileName;
						t.ContentType = file.ContentType;
						t.FileContent = buffer;
						db.Documents.Add(t);
						db.SaveChanges();
					}
				}
			}
			else
			{
				ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "nameProject");
				ViewBag.typeDocumentID = new SelectList(db.TypeDocuments, "typeDocumentID", "typeDocumentName");
				return Content("Select File");
			}

			return RedirectToAction("List");
		}

		// Details
		public ActionResult Details(int id = 0)
		{
			ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "nameProject");
			ViewBag.typeDocumentID = new SelectList(db.TypeDocuments, "typeDocumentID", "typeDocumentName");
			return View(db.Documents.Find(id));
		}

		/*// Update
		public ActionResult Edit(int id = 0)
		{
		return View(db.Documents.Find(id));
		}
		*/
		// GET: Projects/Edit/
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Document document = db.Documents.Find(id);
			if (document == null)
			{
				return HttpNotFound();
			}

			ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "nameProject");
			ViewBag.typeDocumentID = new SelectList(db.TypeDocuments, "typeDocumentID", "typeDocumentName");
			return View(document);
		}

		// POST: Documents/Edit/
		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Edit(Document t, HttpPostedFileBase file,
			[Bind(Include = "DocumentID, Doc, FileDName, typeDocumentID, ProjectID")]
			Document document)
		{
			if (file != null)
			{
				string sd = file.FileName.Substring(file.FileName.Length - 3);
				string p = string.Empty;
				p = Server.MapPath("~/Files/");
				file.SaveAs(p + file.FileName);
				using (db)
				{
					t.Doc = file.FileName;
					db.Entry(t).State = EntityState.Modified;
					db.SaveChanges();
				}
			}

			ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "nameProject");
			ViewBag.typeDocumentID = new SelectList(db.TypeDocuments, "typeDocumentID", "typeDocumentName");
			return View(document);
		}

		public ActionResult Delete(int id = 0)
		{
			return View(db.Documents.Find(id));
		}

		[HttpPost, ActionName("Delete")]
		public ActionResult delete_conf(int id = 0)
		{
			Document t = db.Documents.Find(id);
			db.Documents.Remove(t);
			db.SaveChanges();
			return RedirectToAction("List");
		}
	}
}
