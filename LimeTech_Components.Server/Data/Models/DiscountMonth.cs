﻿using System.ComponentModel.DataAnnotations;

namespace LimeTech_Components.Server.Data.Models
{
    public class DiscountMonth
    {
        public int Id { get; set; }

        [Required]
        public string Month { get; set; }

        [Required]
        public int Discount { get; set; }

        public IEnumerable<Component> Components { get; init; } = new List<Component>();
    }
}