namespace Invoices.DataProcessor.ImportDto.Json
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;
    using static Common.EntityValidation;

    public class ImportProductDto
    {
        [Required]
        [JsonProperty("Name")]
        [MinLength(MinProductNameLength)]
        [MaxLength(MaxProductNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [JsonProperty("Price")]
        [Range((double)MinProductPriceRange, (double)MaxProductPriceRange)]
        public decimal Price { get; set; }

        [Required]
        [JsonProperty("CategoryType")]
        [Range(MinProductEnumRange, MaxProductEnumRange)]
        public int CategoryType { get; set; }

        [Required]
        [JsonProperty("Clients")]
        public int[] Clients { get; set; } = null!;
    }
}
