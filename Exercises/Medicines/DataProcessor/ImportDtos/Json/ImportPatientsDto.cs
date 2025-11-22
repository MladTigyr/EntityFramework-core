namespace Medicines.DataProcessor.ImportDtos.Json
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using static EntityValidation;

    public class ImportPatientsDto
    {
        [Required]
        [JsonProperty("FullName")]
        [MinLength(MinPatientFullNameLength)]
        [MaxLength(MaxPatientFullNameLength)]
        public string FullName { get; set; } = null!;

        [Required]
        [JsonProperty("AgeGroup")]
        [Range(MinAgeValue, MaxAgeValue)]
        public int AgeGroup { get; set; }

        [Required]
        [JsonProperty("Gender")]
        [Range(MinGenderValue, MaxGenderValue)]
        public int Gender { get; set; }

        [Required]
        [JsonProperty("Medicines")]
        public int[] Medicines { get; set; } = null!;
    }
}
