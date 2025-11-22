using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.DataProcessor.ImportDtos.Json
{
    using static EntityValidation.EntityValidation;

    public class ImportBookingsDto
    {
        [Required]
        [JsonProperty("BookingDate")]
        public string BookingDate { get; set; } = null!;

        [Required]
        [JsonProperty("CustomerName")]
        [MinLength(MinCustomerFullNameLength)]
        [MaxLength(MaxCustomerFullNameLength)]
        public string CustomerName { get; set; } = null!;

        [Required]
        [JsonProperty("TourPackageName")]
        [MinLength(MinPackageNameLength)]
        [MaxLength(MaxPackageNameLength)]
        public string TourPackageName { get; set; } = null!;
    }
}
