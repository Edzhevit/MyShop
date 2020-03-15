using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.WebUI.Controllers;
using MyShop.WebUI.Tests.Mocks;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexPageDoesNotReturnProducts()
        {
            IRepository<Product> productRepository = new MockRepository<Product>();
            IRepository<ProductCategory> categoryRepository = new MockRepository<ProductCategory>();

            productRepository.Insert(new Product());

            // Arrange
            HomeController controller = new HomeController(productRepository, categoryRepository);

            // Act
            ViewResult result = controller.Index() as ViewResult;
            var viewModel = (ProductListViewModel)result.ViewData.Model;

            // Assert
            Assert.AreEqual(1, viewModel.Products.Count());
        }
    }
}
