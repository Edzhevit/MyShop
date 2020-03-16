﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;

namespace MyShop.Services.Services
{
    public class OrderService : IOrderService
    {
        private IRepository<Order> orderRepository;

        public OrderService(IRepository<Order> orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public void CreateOrder(Order order, List<BasketItemViewModel> basketItems)
        {
            foreach (var item in basketItems)
            {
                order.OrderItems.Add(new OrderItem()
                {
                    ProductId = item.Id,
                    Image = item.Image,
                    Price = item.Price,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                });
            }

            orderRepository.Insert(order);
            orderRepository.Commit();
        }

        public List<Order> GetOrderList()
        {
            return orderRepository.Collection().ToList();
        }

        public Order GetOrder(string id)
        {
            return orderRepository.Find(id);
        }

        public void Update(Order updatedOrder)
        {
            orderRepository.Update(updatedOrder);
            orderRepository.Commit();
        }
    }
}
