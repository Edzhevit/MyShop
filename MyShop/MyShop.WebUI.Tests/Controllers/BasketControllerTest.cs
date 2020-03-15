using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
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

        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            //Arrange
            IRepository<Basket> basketRepository = new MockRepository<Basket>();
            IRepository<Product> productRepository = new MockRepository<Product>();

            productRepository.Insert(new Product(){Id = "1", Price = 10.00m});
            productRepository.Insert(new Product(){Id = "2", Price = 5.00m});

            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem(){ProductId = "1", Quantity = 2});
            basket.BasketItems.Add(new BasketItem(){ProductId = "2", Quantity = 1});
            basketRepository.Insert(basket);

            IBasketService basketService = new BasketService(productRepository, basketRepository);
            var controller = new BasketController(basketService);
            MockHttpContext httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new HttpCookie("eCommerceBasket"){Value = basket.Id});
            controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);

            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel) result.ViewData.Model;

            Assert.AreEqual(3, basketSummary.BasketCount);
            Assert.AreEqual(25.00m, basketSummary.BasketTotal);
        }
    }
}
