namespace Artillery.DataProcessor.ImportDto.Xml
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Common.EntityValidation;

    [XmlType("Shell")]
    public class ImportShellDto
    {
        [XmlElement("ShellWeight")]
        [Range(MinShellWeight, MaxShellWeight)]
        public double ShellWeight { get; set; }

        [Required]
        [XmlElement("Caliber")]
        [MinLength(MinShellCaliberLength)]
        [MaxLength(MaxShellCaliberLength)]
        public string Caliber { get; set; } = null!;
    }
}
