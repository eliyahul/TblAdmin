using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using Moq;
using TblAdmin;
using TblAdmin.Areas.Books.Controllers;
using TblAdmin.Areas.Books.Models;
using TblAdmin.Areas.Books.ViewModels.Books;
using TblAdmin.Areas.Base.ViewModels;
using TblAdmin.DAL;
using System.Data.Entity;
using PagedList;

namespace TblAdmin.Tests.Controllers
{
    [TestFixture]
    public class BooksControllerTest
    {
        private Mock<DbSet<Book>> mockSetBooks;
        private Mock<DbSet<Publisher>> mockSetPublishers;
        private Mock<TblAdminContext> mockContext; 
        private BooksController controller;
            
        [SetUp]
        public void init()
        {
            var bookData = new List<Book> 
            { 
                new Book { ID = 1, Name = "BBB", PublisherID = 1 }, 
                new Book { Name = "ZZZ", PublisherID = 2 }, 
                new Book { Name = "AAA", PublisherID = 3 },
                new Book { Name = "DDD" }, 
                new Book { Name = "MMM" }, 
                new Book { Name = "QQQ" },
                new Book { Name = "FFF" }, 
                new Book { Name = "RRR" }, 
                new Book { Name = "HHH" },
                new Book { Name = "EEE" }, 
                new Book { Name = "OOO" }, 
                new Book { Name = "KKK" },
                new Book { Name = "GGG" }, 
                new Book { Name = "NNN" }, 
                new Book { Name = "JJJ" },
                new Book { Name = "SSS" }, 
                new Book { Name = "CCC" }, 
                new Book { Name = "TTT" },
                new Book { Name = "LLL" }, 
                new Book { Name = "III" }, 
 
            }.AsQueryable();

            var publisherData = new List<Publisher> 
            { 
                new Publisher { ID = 1, Name = "P1" }, 
                new Publisher { ID = 2, Name = "P2" }, 
                new Publisher { ID = 3, Name = "P3" },
                new Publisher { ID = 4, Name = "P4" }
            }.AsQueryable();

            mockSetBooks = new Mock<DbSet<Book>>();
            mockSetBooks.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(bookData.Provider);
            mockSetBooks.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(bookData.Expression);
            mockSetBooks.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(bookData.ElementType);
            mockSetBooks.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(bookData.GetEnumerator());
            
            mockSetPublishers = new Mock<DbSet<Publisher>>();
            mockSetPublishers.As<IQueryable<Publisher>>().Setup(m => m.Provider).Returns(publisherData.Provider);
            mockSetPublishers.As<IQueryable<Publisher>>().Setup(m => m.Expression).Returns(publisherData.Expression);
            mockSetPublishers.As<IQueryable<Publisher>>().Setup(m => m.ElementType).Returns(publisherData.ElementType);
            mockSetPublishers.As<IQueryable<Publisher>>().Setup(m => m.GetEnumerator()).Returns(publisherData.GetEnumerator());
            
            mockContext = new Mock<TblAdminContext>();
            mockContext.Setup(c => c.Books).Returns(mockSetBooks.Object);
            mockContext.Setup(c => c.Publishers).Returns(mockSetPublishers.Object);

            controller = new BooksController(mockContext.Object);
        }

        [Test]
        [Ignore("Implementation for this test is not complete yet.")]
        public void AA_example_ignored_testcase_to_keep_the_ignore_attribute_handy()
        {
        }

        [Test]
        public void List_with_no_params_displays_first_page_sorted_by_name()
        {
            // Arrange
            string searchString = "";
            string sortCol = "name"; 
            string sortColOrder = "asc";
            int page = 1;
            int pageSize = 3;
            SearchSortPageViewModel sspVM = new SearchSortPageViewModel(searchString, sortCol, sortColOrder, page, pageSize);
            
                
            // Act
            ViewResult result = controller.Index(sspVM) as ViewResult;
            IndexViewModel indexVM = (IndexViewModel) result.ViewData.Model;
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, indexVM.Books.Count);
            Assert.AreEqual("AAA", indexVM.Books[0].Name);
            Assert.AreEqual("CCC", indexVM.Books[2].Name);
        }

        [Test]
        public void List_with_no_params_display_second_page_sorted_by_name()
        {
            // Arrange
            string searchString = "";
            string sortCol = "name";
            string sortColOrder = "asc";
            int page = 2;
            int pageSize = 3;
            SearchSortPageViewModel sspVM = new SearchSortPageViewModel(searchString, sortCol, sortColOrder, page, pageSize);


            // Act
            ViewResult result = controller.Index(sspVM) as ViewResult;
            IndexViewModel indexVM = (IndexViewModel)result.ViewData.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, indexVM.Books.Count);
            Assert.AreEqual("DDD", indexVM.Books[0].Name);
            Assert.AreEqual("FFF", indexVM.Books[2].Name);
        }

        [Test]
        public void List_with_no_params_display_last_page_sorted_by_name()
        {
            // Arrange
            string searchString = "";
            string sortCol = "name";
            string sortColOrder = "asc";
            int page = 4;
            int pageSize = 5;
            SearchSortPageViewModel sspVM = new SearchSortPageViewModel(searchString, sortCol, sortColOrder, page, pageSize);


            // Act
            ViewResult result = controller.Index(sspVM) as ViewResult;
            IndexViewModel indexVM = (IndexViewModel)result.ViewData.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, indexVM.Books.Count);
            Assert.AreEqual("ZZZ", indexVM.Books[4].Name);
        }

        [Test]
        public void List_with_no_params_display_first_page_sorted_by_name_desc()
        {
            // Arrange
            string searchString = "";
            string sortCol = "name";
            string sortColOrder = "desc";
            int page = 1;
            int pageSize = 5;
            SearchSortPageViewModel sspVM = new SearchSortPageViewModel(searchString, sortCol, sortColOrder, page, pageSize);


            // Act
            ViewResult result = controller.Index(sspVM) as ViewResult;
            IndexViewModel indexVM = (IndexViewModel)result.ViewData.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, indexVM.Books.Count);
            Assert.AreEqual("ZZZ", indexVM.Books[0].Name);
        }
        [Test]
        public void List_with_no_params_display_first_page_where_user_sets_page_size()
        {
            // Arrange
            string searchString = "";
            string sortCol = "name";
            string sortColOrder = "asc";
            int page = 2;
            int pageSize = 5;
            SearchSortPageViewModel sspVM = new SearchSortPageViewModel(searchString, sortCol, sortColOrder, page, pageSize);


            // Act
            ViewResult result = controller.Index(sspVM) as ViewResult;
            IndexViewModel indexVM = (IndexViewModel)result.ViewData.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(pageSize, indexVM.Books.Count);
            Assert.AreEqual("FFF", indexVM.Books[0].Name);
            Assert.AreEqual("JJJ", indexVM.Books[pageSize - 1].Name);
        }

        [Test]
        public void List_with_no_params_display_first_page_where_hacker_sets_negative_page_params()
        {
            // Arrange
            string searchString = "";
            string sortCol = "name";
            string sortColOrder = "asc";
            int page = -1;
            int pageSize = -1;
            SearchSortPageViewModel sspVM = new SearchSortPageViewModel(searchString, sortCol, sortColOrder, page, pageSize);


            // Act
            ViewResult result = controller.Index(sspVM) as ViewResult;
            IndexViewModel indexVM = (IndexViewModel)result.ViewData.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, indexVM.Books.Count);
            Assert.AreEqual("AAA", indexVM.Books[0].Name);
            Assert.AreEqual("CCC", indexVM.Books[3 - 1].Name);
        }

        [Test]
        public void List_with_no_params_display_first_page_where_hacker_sets_too_large_page_params()
        {
            // Arrange
            string searchString = "";
            string sortCol = "name";
            string sortColOrder = "asc";
            int page = 999999999;
            int pageSize = 999999999;
            SearchSortPageViewModel sspVM = new SearchSortPageViewModel(searchString, sortCol, sortColOrder, page, pageSize);


            // Act
            ViewResult result = controller.Index(sspVM) as ViewResult;
            IndexViewModel indexVM = (IndexViewModel)result.ViewData.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, indexVM.Books.Count);
            Assert.AreEqual("AAA", indexVM.Books[0].Name);
            Assert.AreEqual("CCC", indexVM.Books[3 - 1].Name);
        }

        [Test]
        public void List_with_search_params_for_string_which_exists()
        {
            // Arrange
            string searchString = "JJ";
            string sortCol = "name";
            string sortColOrder = "asc";
            int page = 1;
            int pageSize = 5;
            SearchSortPageViewModel sspVM = new SearchSortPageViewModel(searchString, sortCol, sortColOrder, page, pageSize);


            // Act
            ViewResult result = controller.Index(sspVM) as ViewResult;
            IndexViewModel indexVM = (IndexViewModel)result.ViewData.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, indexVM.Books.Count);
            Assert.AreEqual("JJJ", indexVM.Books[0].Name);
        }

        [Test]
        public void List_with_search_params_for_string_which_does_not_exist()
        {
            // Arrange
            string searchString = "H3J";
            string sortCol = "name";
            string sortColOrder = "asc";
            int page = 1;
            int pageSize = 5;
            SearchSortPageViewModel sspVM = new SearchSortPageViewModel(searchString, sortCol, sortColOrder, page, pageSize);


            // Act
            ViewResult result = controller.Index(sspVM) as ViewResult;
            IndexViewModel indexVM = (IndexViewModel)result.ViewData.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, indexVM.Books.Count);
        }

        [Test]
        public void List_returns_BadRequest_StatusCode_on_null_input_param()
        {
            // Arrange

            // Act
            //ViewResult result = controller.Details(null) as ViewResult;  // don't use - result is always null for some reason??!!
            var result = controller.Index(null);

            // Assert
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);

        }

        // Details Action


        [Test]
        public void Details_passes_correct_book_and_searchSortPageParams_to_view()
        {
            // Arrange
            string searchString = "H3J";
            string sortCol = "name";
            string sortColOrder = "asc";
            int page = 1;
            int pageSize = 5;
            int id = 1;
            SearchSortPageViewModel sspVM = new SearchSortPageViewModel(searchString, sortCol, sortColOrder, page, pageSize);
            RecordViewModel rVM = new RecordViewModel(sspVM, id);


            // Act
            ViewResult result = controller.Details(rVM) as ViewResult;
            DetailsViewModel detailsVM = (DetailsViewModel)result.ViewData.Model;
            
            // Assert
            Assert.IsNotNull(result);

            Assert.AreEqual(id, detailsVM.Book.ID);
            Assert.AreEqual("BBB", detailsVM.Book.Name);

            Assert.IsNotNull(detailsVM.SearchSortPageParams);
            Assert.AreEqual(searchString, detailsVM.SearchSortPageParams.SearchString);
            Assert.AreEqual(sortCol, detailsVM.SearchSortPageParams.SortCol);
            Assert.AreEqual(sortColOrder, detailsVM.SearchSortPageParams.SortColOrder);
            Assert.AreEqual(page, detailsVM.SearchSortPageParams.Page);
            Assert.AreEqual(pageSize, detailsVM.SearchSortPageParams.PageSize);
        }

        [Test]
        public void Details_returns_BadRequest_StatusCode_on_null_input_param()
        {
            // Arrange

            // Act
            //ViewResult result = controller.Details(null) as ViewResult;  // don't use - result is always null for some reason??!!
            var result = controller.Details(null);

            // Assert
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            
        }

        [Test]
        public void Details_returns_HttpNotFoundResult_on_record_which_doesnt_exist()
        {
            // Arrange
            string searchString = "H3J";
            string sortCol = "name";
            string sortColOrder = "asc";
            int page = 1;
            int pageSize = 5;
            int id = -1;  // a record id which doesn't exist
            SearchSortPageViewModel sspVM = new SearchSortPageViewModel(searchString, sortCol, sortColOrder, page, pageSize);
            RecordViewModel rVM = new RecordViewModel(sspVM, id);

            // Act
            var result = controller.Details(rVM);

            // Assert
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        // Delete action

        [Test]
        public void Delete_GET_passes_correct_book_and_searchSortPageParams_to_view()
        {
            // Arrange
            string searchString = "H3J";
            string sortCol = "name";
            string sortColOrder = "asc";
            int page = 1;
            int pageSize = 5;
            int id = 1;
            SearchSortPageViewModel sspVM = new SearchSortPageViewModel(searchString, sortCol, sortColOrder, page, pageSize);
            RecordViewModel rVM = new RecordViewModel(sspVM, id);


            // Act
            ViewResult result = controller.Delete(rVM) as ViewResult;
            DeleteInputModel deleteVM = (DeleteInputModel)result.ViewData.Model;

            // Assert
            Assert.IsNotNull(result);

            Assert.AreEqual(id, deleteVM.Book.ID);
            Assert.AreEqual("BBB", deleteVM.Book.Name);

            Assert.IsNotNull(deleteVM.SearchSortPageParams);
            Assert.AreEqual(searchString, deleteVM.SearchSortPageParams.SearchString);
            Assert.AreEqual(sortCol, deleteVM.SearchSortPageParams.SortCol);
            Assert.AreEqual(sortColOrder, deleteVM.SearchSortPageParams.SortColOrder);
            Assert.AreEqual(page, deleteVM.SearchSortPageParams.Page);
            Assert.AreEqual(pageSize, deleteVM.SearchSortPageParams.PageSize);
        }

        [Test]
        public void Delete_returns_BadRequest_StatusCode_on_null_input_param()
        {
            // Arrange

            // Act
            //ViewResult result = controller.Details(null) as ViewResult;  // don't use - result is always null for some reason??!!
            var result = controller.Delete(null);

            // Assert
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);

        }

        [Test]
        public void Delete_returns_HttpNotFoundResult_on_record_which_doesnt_exist()
        {
            // Arrange
            string searchString = "H3J";
            string sortCol = "name";
            string sortColOrder = "asc";
            int page = 1;
            int pageSize = 5;
            int id = -1;  // a record id which doesn't exist
            SearchSortPageViewModel sspVM = new SearchSortPageViewModel(searchString, sortCol, sortColOrder, page, pageSize);
            RecordViewModel rVM = new RecordViewModel(sspVM, id);

            // Act
            var result = controller.Delete(rVM);

            // Assert
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void Delete_returns_DeleteInputModel_on_record_which_exists()
        {
            // Arrange
            string searchString = "H3J";
            string sortCol = "name";
            string sortColOrder = "asc";
            int page = 1;
            int pageSize = 5;
            int id = 1;  // a record id which exists
            SearchSortPageViewModel sspVM = new SearchSortPageViewModel(searchString, sortCol, sortColOrder, page, pageSize);
            RecordViewModel rVM = new RecordViewModel(sspVM, id);

            // Act
            ViewResult result = controller.Delete(rVM) as ViewResult;
            DeleteInputModel deleteVM = (DeleteInputModel)result.ViewData.Model;

            // Assert
            Assert.IsInstanceOf<DeleteInputModel>(deleteVM);
        }


        // DeleteConfirmed action

        [Test]
        public void DeleteConfirmed_returns_BadRequest_StatusCode_on_null_input_param()
        {
            // Arrange

            // Act
            //ViewResult result = controller.Details(null) as ViewResult;  // don't use - result is always null for some reason??!!
            var result = controller.DeleteConfirmed(null);

            // Assert
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);

        }

        [Test]
        public void DeleteConfirmed_returns_HttpNotFoundResult_on_record_which_doesnt_exist()
        {
            // Arrange
            string searchString = "H3J";
            string sortCol = "name";
            string sortColOrder = "asc";
            int page = 1;
            int pageSize = 5;
            int id = -1;  // a record id which doesn't exist
            SearchSortPageViewModel sspVM = new SearchSortPageViewModel(searchString, sortCol, sortColOrder, page, pageSize);
            RecordViewModel rVM = new RecordViewModel(sspVM, id);

            // Act
            var result = controller.DeleteConfirmed(rVM);

            // Assert
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void DeleteConfirm_calls_EF_to_delete_record_with_correct_id()
        {
            // Arrange
            string searchString = "H3J";
            string sortCol = "name";
            string sortColOrder = "asc";
            int page = 1;
            int pageSize = 5;
            int id = 1;  // a record id which exists
            SearchSortPageViewModel sspVM = new SearchSortPageViewModel(searchString, sortCol, sortColOrder, page, pageSize);
            RecordViewModel rVM = new RecordViewModel(sspVM, id);

            // Act
            RedirectToRouteResult result = controller.DeleteConfirmed(rVM) as RedirectToRouteResult;

            // Assert
            mockSetBooks.Verify(m => m.Remove(It.IsAny<Book>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());

            Assert.IsInstanceOf<RedirectToRouteResult>(result);

            Assert.AreEqual(result.RouteValues["Controller"], "Books");
            Assert.AreEqual(result.RouteValues["Action"], "Index");

            Assert.AreEqual(result.RouteValues["searchString"], searchString);
            Assert.AreEqual(result.RouteValues["sortCol"], sortCol);
            Assert.AreEqual(result.RouteValues["sortColOrder"], sortColOrder);
            Assert.AreEqual(result.RouteValues["page"], page);
            Assert.AreEqual(result.RouteValues["pageSize"], pageSize);
            
            // How can we test the parameters that it passes to the redirect to check for proper SearchSortPage values?
            
        }


        // Create action


        [Test]
        [Ignore("Implementation for this test is not complete yet.")]
        public void Create_with_no_args_returns_selectlist_of_books()
        {
            // Arrange
            
            // Act
            //var result = controller.Create();
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<SelectList>(result.ViewBag.PublisherID);
            //Assert.AreEqual(result.ViewBag.PublisherID.Count(), 4);
            //Assert.AreEqual(result.ViewBag.PublisherID.Items.Count(), 4);
            //Assert.IsInstanceOf<Publisher>(result.ViewBag.PublisherID.Items);
            // How can I tell if the items in the selectList are publishers?
            foreach (var item in result.ViewBag.PublisherID.Items)
            {
                Console.WriteLine(item);
            }
            //Console.WriteLine(typeof(result.ViewBag.PublisherID.Items));
        }

        [Test]
        [Ignore("Implementation for this test is not complete yet.")]
        public void Create_returns()
        {
            //controller.Create();

            //mockSet.Verify(m => m.Add(It.IsAny<Book>()), Times.Once());
            //mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        // Verify if I pass in a new value for created or modified dates to Edit, I cannot change the date in the db.
        // Verify all actions pass correct SearchSortPage params to view, in addition to their other data
        
    }
}
