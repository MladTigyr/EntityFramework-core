using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs.Import
{
    public class CategoryDto
    {
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; } = null!;
    }
}
