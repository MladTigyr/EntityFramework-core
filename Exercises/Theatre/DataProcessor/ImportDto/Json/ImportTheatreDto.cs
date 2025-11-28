namespace Theatre.DataProcessor.ImportDto.Json
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;

    public class ImportTheatreDto
    {
        [Required]
        [JsonProperty("Name")]
        [MinLength(MinTheatreNameLength)]
        [MaxLength(MaxTheatreNameLength)]
        public string Name { get; set; } = null!;

        [JsonProperty("NumberOfHalls")]
        [Range(MinTheatreNumberOfHalls, MaxTheatreNumberOfHalls)]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [JsonProperty("Director")]
        [MinLength(MinTheatreDirectorLength)]
        [MaxLength(MaxTheatreDirectorLength)]
        public string Director { get; set; } = null!;

        [Required]
        [JsonProperty("Tickets")]
        public ImportTicketDto[] Tickets { get; set; } = null!;
    }
}
