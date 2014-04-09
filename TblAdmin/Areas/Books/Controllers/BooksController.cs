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
    public class BooksController : Controller
    {
        private TblAdminContext db = new TblAdminContext();

        // GET: Books/Books
        public ActionResult Index(string sort)
        {
            var books = db.Books.Include(b => b.Publisher);

            // retrieve the books according to the specified sort order
            switch (sort)
            {
                case "name_desc":
                    books = books.OrderByDescending(b => b.Name);
                    break;
                case "createdDate_asc":
                    books = books.OrderBy(b => b.CreatedDate);
                    break;
                case "createdDate_desc":
                    books = books.OrderByDescending(b => b.CreatedDate);
                    break;
                case "modifiedDate_asc":
                    books = books.OrderBy(b => b.ModifiedDate);
                    break;
                case "modifiedDate_desc":
                    books = books.OrderByDescending(b => b.ModifiedDate);
                    break;
                case "publisher_asc":
                    books = books.OrderBy(b => b.Publisher.Name);
                    break;
                case "publisher_desc":
                    books = books.OrderByDescending(b => b.Publisher.Name);
                    break;
                default:
                    books = books.OrderBy(b => b.Name);
                    break;
            }

            // Toggle the next sort
            ViewBag.NextNameSort = (string.IsNullOrEmpty(sort)) ? "name_desc" : "";
            ViewBag.NextCreatedDateSort = (sort == "createdDate_asc") ? "createdDate_desc" : "createdDate_asc";
            ViewBag.NextModifiedDateSort = (sort == "modifiedDate_asc") ? "modifiedDate_desc" : "modifiedDate_asc";
            ViewBag.NextPublisherSort = (sort == "publisher_asc") ? "publisher_desc" : "publisher_asc";

            return View(books.ToList());
        }

        // GET: Books/Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Books/Create
        public ActionResult Create()
        {
            ViewBag.PublisherID = new SelectList(db.Publishers, "ID", "Name");
            return View();
        }

        // POST: Books/Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,CreatedDate,ModifiedDate,PublisherID")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PublisherID = new SelectList(db.Publishers, "ID", "Name", book.PublisherID);
            return View(book);
        }

        // GET: Books/Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.PublisherID = new SelectList(db.Publishers, "ID", "Name", book.PublisherID);
            return View(book);
        }

        // POST: Books/Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,CreatedDate,ModifiedDate,PublisherID")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PublisherID = new SelectList(db.Publishers, "ID", "Name", book.PublisherID);
            return View(book);
        }

        // GET: Books/Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
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
