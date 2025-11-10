using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs.Import
{
    public class CarsDto
    {
        [Required]
        [JsonProperty("make")]
        public string Make { get; set; } = null!;

        [Required]
        [JsonProperty("model")]
        public string Model { get; set; } = null!;

        [JsonProperty("traveledDistance")]
        public long TraveledDistance { get; set; }

        [JsonProperty("partsId")]
        public int[] PartsId { get; set; } = null!;
    }
}
