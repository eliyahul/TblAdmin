using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TblAdmin.Areas.Books.Models;
using TblAdmin.Areas.Books.ViewModels;
using TblAdmin.Areas.Base.Controllers;
using TblAdmin.DAL;
using PagedList;

namespace TblAdmin.Areas.Books.Controllers
{
    public class BooksController : BaseController
    {
        private TblAdminContext db;

        public BooksController(TblAdminContext context)
        {
            db = context;
        }

        // GET: Books/Books
        public ActionResult Index(IndexViewModel vm)
        {
            IQueryable<Book> books = from b in db.Books select b; //db.Books.Include(b => b.Publisher); // the "Include" messes up mocking DbSet with mock.
            if (!String.IsNullOrEmpty(vm.SearchString))
            {
                books = books.Where(s => s.Name.ToUpper().Contains(vm.SearchString.ToUpper()));
            }

            switch (vm.SortCol)
            {
                case "name":
                    if (vm.SortOrder == "desc")
                    {
                        books = books.OrderByDescending(b => b.Name);
                    }
                    else
                    {
                        books = books.OrderBy(b => b.Name);
                    }
                    break;
                case "createdDate":
                    if (vm.SortOrder == "desc")
                    {
                        books = books.OrderByDescending(b => b.CreatedDate);
                    }
                    else
                    {
                        books = books.OrderBy(b => b.CreatedDate);
                    }
                    break;
                case "modifiedDate":
                    if (vm.SortOrder == "desc")
                    {
                        books = books.OrderByDescending(b => b.ModifiedDate);
                    }
                    else
                    {
                        books = books.OrderBy(b => b.ModifiedDate);
                    }
                    break;
                case "publisher":
                    if (vm.SortOrder == "desc")
                    {
                        books = books.OrderByDescending(b => b.Publisher.Name);
                    }
                    else
                    {
                        books = books.OrderBy(b => b.Publisher.Name);
                    }
                    break;
                default:
                    if (vm.SortOrder == "desc")
                    {
                        books = books.OrderByDescending(b => b.Name);
                    }
                    else
                    {
                        books = books.OrderBy(b => b.Name);
                    }
                    break;
            }

            vm.NextSortOrder = (vm.SortOrder == "desc") ? "asc" : "desc"; 
            vm.Books = books.ToPagedList(vm.Page, vm.PageSize);
            
            return View(vm);
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
                return this.RedirectToAction<BooksController>(c => c.Index(null));
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
                return this.RedirectToAction<BooksController>(c => c.Index(null));
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
            return this.RedirectToAction<BooksController>(c => c.Index(null));
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
