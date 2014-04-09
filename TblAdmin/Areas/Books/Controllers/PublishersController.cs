using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TblAdmin.Areas.Books.Models;
using TblAdmin.DAL;

namespace TblAdmin.Areas.Books.Controllers
{
    public class PublishersController : Controller
    {
        private TblAdminContext db = new TblAdminContext();

        // GET: Books/Publishers
        public ActionResult Index(string sort, string searchString)
        {
            var publishers = from p in db.Publishers
                           select p;

            // Filter according to searchString
            if (!String.IsNullOrEmpty(searchString))
            {
                publishers = publishers.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
            }

            // retrieve the publishers according to the specified sort order
            switch (sort)
            {
                case "name_desc":
                    publishers = publishers.OrderByDescending(p => p.Name);
                    break;
                case "createdDate_asc":
                    publishers = publishers.OrderBy(p => p.CreatedDate);
                    break;
                case "createdDate_desc":
                    publishers = publishers.OrderByDescending(p => p.CreatedDate);
                    break;
                case "modifiedDate_asc":
                    publishers = publishers.OrderBy(p => p.ModifiedDate);
                    break;
                case "modifiedDate_desc":
                    publishers = publishers.OrderByDescending(p => p.ModifiedDate);
                    break;
                default:
                    publishers = publishers.OrderBy(p => p.Name);
                    break;
            }

            // Toggle the next sort
            ViewBag.NextNameSort = (string.IsNullOrEmpty(sort)) ? "name_desc" : "";
            ViewBag.NextCreatedDateSort = (sort == "createdDate_asc") ? "createdDate_desc" : "createdDate_asc";
            ViewBag.NextModifiedDateSort = (sort == "modifiedDate_asc") ? "modifiedDate_desc" : "modifiedDate_asc";
            ViewBag.SearchString = searchString;

            return View(publishers.ToList());
        }

        // GET: Books/Publishers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publisher publisher = db.Publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // GET: Books/Publishers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Publishers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,CreatedDate,ModifiedDate")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                db.Publishers.Add(publisher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(publisher);
        }

        // GET: Books/Publishers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publisher publisher = db.Publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // POST: Books/Publishers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,CreatedDate,ModifiedDate")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publisher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(publisher);
        }

        // GET: Books/Publishers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publisher publisher = db.Publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // POST: Books/Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Publisher publisher = db.Publishers.Find(id);
            db.Publishers.Remove(publisher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
