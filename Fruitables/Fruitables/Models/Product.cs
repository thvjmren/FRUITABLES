﻿using Fruitables.Models.Base;

namespace Fruitables.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        
        public List<ProductImage> Images { get; set; } 
    }
}
