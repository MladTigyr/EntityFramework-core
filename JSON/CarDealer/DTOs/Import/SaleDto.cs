using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs.Import
{
    public class SaleDto
    {
        [Required]
        [JsonProperty("carId")]
        public string CarId { get; set; } = null!;

        [Required]
        [JsonProperty("customerId")]
        public string CustomerId { get; set; } = null!;

        [Required]
        [JsonProperty("discount")]
        public string Discount { get; set; } = null!;
    }
}
