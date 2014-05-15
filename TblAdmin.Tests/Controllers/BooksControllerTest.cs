using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using Moq;
using TblAdmin;
using TblAdmin.Areas.Books.Controllers;
using TblAdmin.Areas.Books.Models;
using TblAdmin.Areas.Books.ViewModels;
using TblAdmin.DAL;
using System.Data.Entity;
using PagedList;

namespace TblAdmin.Tests.Controllers
{
    [TestFixture]
    public class BooksControllerTest
    {
        private BooksController controller;

        [SetUp]
        public void init()
        {
            var data = new List<Book> 
            { 
                new Book { Name = "BBB" }, 
                new Book { Name = "ZZZ" }, 
                new Book { Name = "AAA" },
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

            var mockSet = new Mock<DbSet<Book>>();
            mockSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            
            var mockContext = new Mock<TblAdminContext>();
            mockContext.Setup(c => c.Books).Returns(mockSet.Object);
            mockContext.Setup(c => c.Books).Returns(mockSet.Object);
            controller = new BooksController(mockContext.Object);
        }

        [Test]
        public void List_with_no_params_displays_first_page_sorted_by_name()
        {
            // Arrange
            string searchString = "";
            string sortCol = "name"; 
            string sortOrder = "asc"; 
            string nextSortOrder = "desc";
            int page = 1;
            int pageSize = 3;
            PagedList.IPagedList<Book> books = null;
            IndexViewModel indexVM = new IndexViewModel(searchString, sortCol, sortOrder, nextSortOrder, page, pageSize, books);
                
            // Act
            ViewResult result = controller.Index(indexVM) as ViewResult;
            indexVM = (IndexViewModel) result.ViewData.Model;
            
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
            string sortOrder = "asc";
            string nextSortOrder = "desc";
            int page = 2;
            int pageSize = 3;
            PagedList.IPagedList<Book> books = null;
            IndexViewModel indexVM = new IndexViewModel(searchString, sortCol, sortOrder, nextSortOrder, page, pageSize, books);

            // Act
            ViewResult result = controller.Index(indexVM) as ViewResult;
            indexVM = (IndexViewModel)result.ViewData.Model;

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
            string sortOrder = "asc";
            string nextSortOrder = "desc";
            int page = 4;
            int pageSize = 5;
            PagedList.IPagedList<Book> books = null;
            IndexViewModel indexVM = new IndexViewModel(searchString, sortCol, sortOrder, nextSortOrder, page, pageSize, books);

            // Act
            ViewResult result = controller.Index(indexVM) as ViewResult;
            indexVM = (IndexViewModel)result.ViewData.Model;

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
            string sortOrder = "desc";
            string nextSortOrder = "asc";
            int page = 1;
            int pageSize = 5;
            PagedList.IPagedList<Book> books = null;
            IndexViewModel indexVM = new IndexViewModel(searchString, sortCol, sortOrder, nextSortOrder, page, pageSize, books);

            // Act
            ViewResult result = controller.Index(indexVM) as ViewResult;
            indexVM = (IndexViewModel)result.ViewData.Model;

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
            string sortOrder = "asc";
            string nextSortOrder = "desc";
            int page = 2;
            int pageSize = 5;
            PagedList.IPagedList<Book> books = null;
            IndexViewModel indexVM = new IndexViewModel(searchString, sortCol, sortOrder, nextSortOrder, page, pageSize, books);

            // Act
            ViewResult result = controller.Index(indexVM) as ViewResult;
            indexVM = (IndexViewModel)result.ViewData.Model;

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
            string sortOrder = "asc";
            string nextSortOrder = "desc";
            int page = -1;
            int pageSize = -1;
            PagedList.IPagedList<Book> books = null;
            IndexViewModel indexVM = new IndexViewModel(searchString, sortCol, sortOrder, nextSortOrder, page, pageSize, books);

            // Act
            ViewResult result = controller.Index(indexVM) as ViewResult;
            indexVM = (IndexViewModel)result.ViewData.Model;

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
            string sortOrder = "asc";
            string nextSortOrder = "desc";
            int page = 999999999;
            int pageSize = 999999999;
            PagedList.IPagedList<Book> books = null;
            IndexViewModel indexVM = new IndexViewModel(searchString, sortCol, sortOrder, nextSortOrder, page, pageSize, books);

            // Act
            ViewResult result = controller.Index(indexVM) as ViewResult;
            indexVM = (IndexViewModel)result.ViewData.Model;

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
            string sortOrder = "asc";
            string nextSortOrder = "desc";
            int page = 1;
            int pageSize = 5;
            PagedList.IPagedList<Book> books = null;
            IndexViewModel indexVM = new IndexViewModel(searchString, sortCol, sortOrder, nextSortOrder, page, pageSize, books);

            // Act
            ViewResult result = controller.Index(indexVM) as ViewResult;
            indexVM = (IndexViewModel)result.ViewData.Model;

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
            string sortOrder = "asc";
            string nextSortOrder = "desc";
            int page = 1;
            int pageSize = 5;
            PagedList.IPagedList<Book> books = null;
            IndexViewModel indexVM = new IndexViewModel(searchString, sortCol, sortOrder, nextSortOrder, page, pageSize, books);

            // Act
            ViewResult result = controller.Index(indexVM) as ViewResult;
            indexVM = (IndexViewModel)result.ViewData.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, indexVM.Books.Count);
        }
        
    }
}
