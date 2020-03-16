using System.Collections.Generic;

namespace MyShop.Core.Models
{
    public class Order : BaseEntity
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string OrderStatus { get; set; }
        public virtual ICollection<OrderItem> OrderItems{ get; set; }

        public Order()
        {
            this.OrderItems = new List<OrderItem>();
        }
    }
}
