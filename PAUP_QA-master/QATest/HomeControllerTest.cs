using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Controllers;
using QA.Models;

namespace QATest
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void TestIndex()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(String.Empty, result.ViewName);
        }
        [TestMethod]
        public void TestIndexTitle()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Index() as ViewResult;

            Assert.AreEqual("QBox", result.ViewBag.Title);

        }
    }
}
