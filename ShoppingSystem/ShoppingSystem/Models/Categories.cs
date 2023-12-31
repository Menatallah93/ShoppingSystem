﻿using ShoppingSystem.Repository;

namespace ShoppingSystem.Models
{
    public class Categories 
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Product>? product { get; set; }
    }
}
