using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using TblAdmin.Areas.Books.Models;
using TblAdmin.Areas.Books.ViewModels.Books;
using TblAdmin.Areas.Base.Controllers;
using TblAdmin.Areas.Base.ViewModels;
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
        public ActionResult Index(SearchSortPageViewModel vm)
        {
            if (vm.ToggleOrder)
            {
                vm.CurrentOrder = vm.NextOrder;
                vm.NextOrder = (vm.NextOrder == SearchSortPageViewModel.SORT_ORDER_ASC) ? SearchSortPageViewModel.SORT_ORDER_DESC : SearchSortPageViewModel.SORT_ORDER_ASC;
            }
            
            SearchSortPageViewModel publisherRouteParams, nameRouteParams, createdDateRouteParams, modifiedDateRouteParams;
            SearchSortPageViewModel createLinkRouteParams;

            publisherRouteParams = new SearchSortPageViewModel();
            publisherRouteParams.SearchString = vm.SearchString;
            publisherRouteParams.SortCol = "publisher";
            nameRouteParams = new SearchSortPageViewModel();
            nameRouteParams.SearchString = vm.SearchString;
            nameRouteParams.SortCol = "name";
            createdDateRouteParams = new SearchSortPageViewModel();
            createdDateRouteParams.SearchString = vm.SearchString;
            createdDateRouteParams.SortCol = "createdDate";
            modifiedDateRouteParams = new SearchSortPageViewModel();
            modifiedDateRouteParams.SearchString = vm.SearchString;
            modifiedDateRouteParams.SortCol = "modifiedDate";

            IQueryable<Book> books = from b in db.Books select b; //db.Books.Include(b => b.Publisher); // the "Include" messes up mocking DbSet with mock.
            if (!String.IsNullOrEmpty(vm.SearchString))
            {
                books = books.Where(s => s.Name.ToUpper().Contains(vm.SearchString.ToUpper()));
            }

            switch (vm.SortCol)
            {
                case "name":
                    if (vm.CurrentOrder == SearchSortPageViewModel.SORT_ORDER_DESC)
                    {
                        books = books.OrderByDescending(b => b.Name);
                    }
                    else
                    {
                        books = books.OrderBy(b => b.Name);
                    }

                    nameRouteParams = (SearchSortPageViewModel) vm.ShallowCopy();
                    nameRouteParams.ToggleOrder = true;
                    break;
                case "createdDate":
                    if (vm.CurrentOrder == SearchSortPageViewModel.SORT_ORDER_DESC)
                    {
                        books = books.OrderByDescending(b => b.CreatedDate);
                    }
                    else
                    {
                        books = books.OrderBy(b => b.CreatedDate);
                    }
                    createdDateRouteParams = (SearchSortPageViewModel) vm.ShallowCopy();
                    createdDateRouteParams.ToggleOrder = true;
                    break;
                case "modifiedDate":
                    if (vm.CurrentOrder == SearchSortPageViewModel.SORT_ORDER_DESC)
                    {
                        books = books.OrderByDescending(b => b.ModifiedDate);
                    }
                    else
                    {
                        books = books.OrderBy(b => b.ModifiedDate);
                    }
                    modifiedDateRouteParams = (SearchSortPageViewModel) vm.ShallowCopy();
                    modifiedDateRouteParams.ToggleOrder = true;
                    break;
                case "publisher":
                    if (vm.CurrentOrder == SearchSortPageViewModel.SORT_ORDER_DESC)
                    {
                        books = books.OrderByDescending(b => b.Publisher.Name);
                    }
                    else
                    {
                        books = books.OrderBy(b => b.Publisher.Name);
                    }
                    publisherRouteParams = (SearchSortPageViewModel) vm.ShallowCopy();
                    publisherRouteParams.ToggleOrder = true;
                    break;
                default:
                    books = books.OrderBy(b => b.Name);
                    break;
            }

            createLinkRouteParams = (SearchSortPageViewModel)vm.ShallowCopy();
            createLinkRouteParams.ToggleOrder = false;

            IndexViewModel Ivm = new IndexViewModel(
                vm, 
                books.ToPagedList(vm.Page, vm.PageSize),
                publisherRouteParams,
                nameRouteParams,
                createdDateRouteParams,
                modifiedDateRouteParams,
                createLinkRouteParams
            );
            return View(Ivm);
        }
        
        // GET: Books/Books/Details/5
        public ActionResult Details(RecordViewModel recordVm)
        {
            Book book;
            DetailsViewModel dvm;

            //book = db.Books.Find(recordVm.Id); // Testing issue: don't use find, as it does not work when mocking EF 6
            book = db.Books.FirstOrDefault(i => i.ID == recordVm.Id);
            if (book == null)
            {
                return HttpNotFound();
            }
                
            dvm = new DetailsViewModel(recordVm.SearchSortPageParams, book);
            return View(dvm);
        }

        // GET: Books/Books/Create
        public ActionResult Create(SearchSortPageViewModel searchSortPageParams)
        {
            IEnumerable<SelectListItem> publishers;
            CreateViewModel cvm;

            publishers = new SelectList(db.Publishers.OrderBy(s => s.Name), "ID", "Name");
            cvm = new CreateViewModel(searchSortPageParams, null, publishers);
            return View(cvm);
        }

        // POST: Books/Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateInputModel cim)
        {
            IEnumerable<SelectListItem> publishers;
            CreateViewModel cvm;

            if (ModelState.IsValid)
            {
                db.Books.Add(cim.Book);
                db.SaveChanges();
                return RedirectToActionFor<BooksController>(c => c.Index(null), cim.SearchSortPageParams);
            }
            publishers = new SelectList(db.Publishers.OrderBy(s => s.Name), "ID", "Name", cim.Book.PublisherID);
            cvm = new CreateViewModel(cim.SearchSortPageParams, cim.Book, publishers);
            return View(cvm);
        }

        // GET: Books/Books/Edit/5
        public ActionResult Edit(RecordViewModel recordVm)
        {
            Book book;
            IEnumerable<SelectListItem> publishers; 
            EditViewModel evm;

            book = db.Books.FirstOrDefault(i => i.ID == recordVm.Id);
            if (book == null)
            {
                return HttpNotFound();
            }

            publishers = new SelectList(db.Publishers.OrderBy(s => s.Name), "ID", "Name", book.PublisherID);
            evm = new EditViewModel(recordVm.SearchSortPageParams, book, publishers);
            return View(evm);
        }

        // POST: Books/Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditInputModel eim)
        {
            IEnumerable<SelectListItem> publishers;
            EditViewModel evm;

            if (ModelState.IsValid)
            {
                // here is where we would do mapping.

                db.Entry(eim.Book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToActionFor<BooksController>(c => c.Index(null), eim.SearchSortPageParams);
            }
            publishers = new SelectList(db.Publishers.OrderBy(s => s.Name), "ID", "Name", eim.Book.PublisherID);
            evm = new EditViewModel(eim.SearchSortPageParams, eim.Book, publishers);
            return View(evm);
        }

        // GET: Books/Books/Delete/5
        public ActionResult Delete(RecordViewModel recordVm)
        {
            Book book;
            DeleteViewModel dim;

            book = db.Books.FirstOrDefault(i => i.ID == recordVm.Id);
            if (book == null)
            {
                return HttpNotFound();
            }

            dim = new DeleteViewModel(recordVm.SearchSortPageParams, book);
            
            return View(dim);
        }

        // POST: Books/Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(RecordViewModel recordVm)
        {
            Book book;

            book = db.Books.FirstOrDefault(i => i.ID == recordVm.Id);
            if (book == null)
            {
                return HttpNotFound();
            }

            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToActionFor<BooksController>(c => c.Index(null), recordVm.SearchSortPageParams);
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
