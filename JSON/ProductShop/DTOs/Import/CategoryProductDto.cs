using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs.Import
{
    public class CategoryProductDto
    {
        [Required]
        [JsonProperty("CategoryId")]
        public string CategoryId { get; set; } = null!;

        [Required]
        [JsonProperty("ProductId")]
        public string ProductId { get; set; } = null!;
    }
}
