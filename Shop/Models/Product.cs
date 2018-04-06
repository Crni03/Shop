using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }
        [Required]
        [StringLength(25)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int QuantityForPrice { get; set; }
        public decimal QuantityPrice { get; set; }       

    }
}