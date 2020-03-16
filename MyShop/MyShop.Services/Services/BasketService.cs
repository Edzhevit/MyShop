using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;

namespace MyShop.Services.Services
{
    public class BasketService : IBasketService
    {
        private IRepository<Product> productRepository;
        private IRepository<Basket> basketRepository;

        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> productRepository, IRepository<Basket> basketRepository)
        {
            this.productRepository = productRepository;
            this.basketRepository = basketRepository;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();

            if (cookie != null)
            {
                string basketId = cookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = basketRepository.Find(basketId);
                }
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            basketRepository.Insert(basket);
            basketRepository.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);

            return basket;
        }

        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem basketItem = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if (basketItem == null)
            {
                basketItem = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };
                basket.BasketItems.Add(basketItem);
            }
            else
            {
                basketItem.Quantity++;
            }

            basketRepository.Commit();
        }

        public void RemoveFromBasket(HttpContextBase contextBase, string itemId)
        {
            Basket basket = GetBasket(contextBase, true);
            BasketItem basketItem = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (basketItem != null)
            {
                basket.BasketItems.Remove(basketItem);
                basketRepository.Commit();
            }
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);

            if (basket != null)
            {
                var result = (from b in basket.BasketItems
                    join p in productRepository.Collection() on b.ProductId equals p.Id
                    select new BasketItemViewModel()
                    {
                        Id = b.Id,
                        Quantity = b.Quantity,
                        ProductName = p.Name,
                        Image = p.Image,
                        Price = p.Price
                    }).ToList();

                return result;
            }

            return new List<BasketItemViewModel>();
        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            BasketSummaryViewModel summaryModel = new BasketSummaryViewModel(0, 0);

            if (basket != null)
            {
                int? basketCount = (from item in basket.BasketItems select item.Quantity).Sum();

                decimal basketTotal = (from item in basket.BasketItems
                    join p in productRepository.Collection() on item.ProductId equals p.Id
                    select item.Quantity * p.Price).Sum();

                summaryModel.BasketCount = (int) basketCount;
                summaryModel.BasketTotal = basketTotal;

                return summaryModel;
            }

            return summaryModel;
        }

        public void ClearBasket(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            basket.BasketItems.Clear();
            basketRepository.Commit();
        }
    }
}
