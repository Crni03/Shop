using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class Discount
    {
        [Key]
        public int IdDiscount { get; set; }
        [Required]
        [StringLength(25)]
        public string Name { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public bool Percent { get; set; }
        [Required]
        public bool CanConjunc { get; set; }

    }
}