namespace Boardgames.DataProcessor.ImportDto.Json
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;

    public class ImportSellerDto
    {
        [Required]
        [JsonProperty("Name")]
        [MinLength(MinSellerNameLength)]
        [MaxLength(MaxSellerNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [JsonProperty("Address")]
        [MinLength(MinSellerAddressLength)]
        [MaxLength(MaxSellerAddressLength)]
        public string Address { get; set; } = null!;

        [Required]
        [JsonProperty("Country")]
        public string Country { get; set; } = null!;

        [Required]
        [JsonProperty("Website")]
        [RegularExpression(SellerWebsiteRegex)]
        public string Website { get; set; } = null!;

        [Required]
        [JsonProperty("Boardgames")]
        public int[] Boardgames { get; set; } = null!;
    }
}
