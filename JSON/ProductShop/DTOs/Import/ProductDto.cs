using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs.Import
{
    public class ProductDto
    {
        [Required]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [JsonProperty("Price")]
        public string Price { get; set; } = null!;

        [Required]
        [JsonProperty("SellerId")]
        public string SellerId { get; set; } = null!;

        [JsonProperty("BuyerId")]
        public string? BuyerId { get; set; } 
    }
}
