using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Moq;
using TblAdmin;
using TblAdmin.Controllers;

namespace TblAdmin.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        private HomeController controller;

        [SetUp]
        public void init()
        {
            controller = new HomeController();
            //Mock<HomeController> mock = new Mock<HomeController>(); // just wanted to see if it builds.
        }

        [Test]
        public void Index()
        {
            // Arrange
            // .. add anything specific here for this test, that is not in the global [Setup] routine above.

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void About()
        {
            // Arrange
            // .. add anything specific here for this test, that is not in the global [Setup] routine above.

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Contact()
        {
            // Arrange
            // .. add anything specific here for this test, that is not in the global [Setup] routine above.

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
