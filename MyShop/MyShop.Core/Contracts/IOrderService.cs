using System.Collections.Generic;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;

namespace MyShop.Core.Contracts
{
    public interface IOrderService
    {
        void CreateOrder(Order order, List<BasketItemViewModel> basketItems);
    }
}
