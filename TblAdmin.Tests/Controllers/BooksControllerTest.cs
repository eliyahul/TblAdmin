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
        public void Index()
        {
            // Arrange
            // .. add anything specific here for this test, that is not in the global [Setup] routine above.
            string sort = "name_desc";
            string searchString = "AAA";
            int? page = 1;

            // Act
            ActionResult result = controller.Index(sort, searchString, page) as ActionResult;
            ViewResult vresult = result as ViewResult;
            PagedList.IPagedList<Book> books = vresult.ViewData.Model as PagedList.IPagedList<Book>;
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, books.Count);
            Assert.AreEqual("AAA", books[0].Name);
            //Assert.AreEqual("BBB", books[1].Name);
            //Assert.AreEqual("ZZZ", books[2].Name);
        }

        
    }
}
