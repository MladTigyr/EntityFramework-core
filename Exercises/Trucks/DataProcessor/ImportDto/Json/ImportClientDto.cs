namespace Trucks.DataProcessor.ImportDto.Json
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;

    public class ImportClientDto
    {
        [Required]
        [JsonProperty("Name")]
        [MinLength(MinClientNameLength)]
        [MaxLength(MaxClientNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [JsonProperty("Nationality")]
        [MinLength(MinClientNationalityLength)]
        [MaxLength(MaxClientNationalityLength)]
        public string Nationality { get; set; } = null!;

        [Required]
        [JsonProperty("Type")]
        public string Type { get; set; } = null!;

        [Required]
        [JsonProperty("Trucks")]
        public int[] Trucks { get; set; } = null!;
    }
}
