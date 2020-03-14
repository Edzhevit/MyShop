﻿using System.ComponentModel.DataAnnotations;

namespace MyShop.Core.Models
{
    public class Product : BaseEntity
    {
        [StringLength(20)]
        public  string Name { get; set; }
        public  string Description { get; set; }
        public  decimal Price { get; set; }
        public  string Category { get; set; }
        public  string Image { get; set; }
    }
}
