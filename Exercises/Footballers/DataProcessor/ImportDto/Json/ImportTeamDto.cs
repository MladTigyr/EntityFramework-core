namespace Footballers.DataProcessor.ImportDto.Json
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;
    public class ImportTeamDto
    {
        [Required]
        [JsonProperty("Name")]
        [MinLength(MinTeamNameLength)]
        [MaxLength(MaxTeamNameLength)]
        [RegularExpression(TeamRegex)]
        public string Name { get; set; } = null!;

        [Required]
        [JsonProperty("Nationality")]
        [MinLength(MinTeamNationalityLength)]
        [MaxLength(MaxTeamNationalityLength)]
        public string Nationality { get; set; } = null!;

        [JsonProperty("Trophies")]
        public int Trophies { get; set; }

        [Required]
        [JsonProperty("Footballers")]
        public int[] Footballers { get; set; } = null!;
    }
}
