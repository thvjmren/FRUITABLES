﻿using Fruitables.Models.Base;

namespace Fruitables.Models
{
    public class ProductImage:BaseEntity
    {
        public string Image { get; set; }
        public bool? IsPrimary { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
