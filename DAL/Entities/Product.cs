using System;
using System.Collections.Generic;
using DAL.Entity;

namespace DAL.Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public double Rate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public List<ProductImage> ProductImages { get; set; }

    }
}

