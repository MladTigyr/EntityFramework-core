namespace Artillery.DataProcessor.ImportDto.Json
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;


    public class ImportGunDto
    {
        [JsonProperty("ManufacturerId")]
        public int ManufacturerId { get; set; }

        [JsonProperty("GunWeight")]
        [Range(MinGunWeight, MaxGunWeight)]
        public int GunWeight { get; set; }

        [JsonProperty("BarrelLength")]
        [Range(MinGunBarrelLength, MaxGunBarrelLength)]
        public double BarrelLength { get; set; }

        [JsonProperty("NumberBuild")]
        public int? NumberBuild { get; set; }

        [JsonProperty("Range")]
        [Range(MinGunRange, MaxGunRange)]
        public int Range { get; set; }

        [Required]
        [JsonProperty("GunType")]
        public string GunType { get; set; } = null!;

        [JsonProperty("ShellId")]
        public int ShellId { get; set; }

        [Required]
        [JsonProperty("Countries")]
        public ImportCountryIdDto[] Countries { get; set; } = null!;
    }
}
