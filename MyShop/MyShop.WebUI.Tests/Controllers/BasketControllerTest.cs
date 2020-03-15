using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Services.Services;
using MyShop.WebUI.Controllers;
using MyShop.WebUI.Tests.Mocks;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTest
    {
        [TestMethod]
        public void CanAddBasketItem()
        {
            //Arrange
            IRepository<Basket> basketRepository = new MockRepository<Basket>();
            IRepository<Product> productRepository = new MockRepository<Product>();
            MockHttpContext httpContext = new MockHttpContext();

            IBasketService basketService = new BasketService(productRepository, basketRepository);

            var  controller = new BasketController(basketService);
            controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);

            //Act
            // basketService.AddToBasket(httpContext, "1");
            controller.AddToBasket("1");

            Basket basket = basketRepository.Collection().FirstOrDefault();

            //Assert
            Assert.IsNotNull(basket);
            Assert.AreEqual(1, basket.BasketItems.Count);
            Assert.AreEqual("1", basket.BasketItems.ToList().FirstOrDefault().ProductId);
        }
    }
}
