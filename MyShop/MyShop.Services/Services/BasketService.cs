using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyShop.Core.Contracts;
using MyShop.Core.Models;

namespace MyShop.Services.Services
{
    public class BasketService
    {
        private IRepository<Product> productRepository;
        private IRepository<Basket> basketRepository;

        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> ProductRepository, IRepository<Basket> BasketRepository)
        {
            this.productRepository = ProductRepository;
            this.basketRepository = BasketRepository;
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
    }
}
