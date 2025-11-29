namespace NetPay.DataProcessor.ImportDtos.Xml
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using static Common.EntityValidation;

    [XmlType("Household")]  
    public class ImportHouseholdDto
    {
        [Required]
        [XmlAttribute("phone")]
        [RegularExpression(HouseholdRegex)]
        public string Phone { get; set; } = null!;

        [Required]
        [XmlElement("ContactPerson")]
        [MinLength(MinHouseholdContactPersonLength)]
        [MaxLength(MaxHouseholdContactPersonLength)]
        public string ContactPerson { get; set; } = null!;

        [XmlElement("Email")]
        [MinLength(MinHouseholdEmailLength)]
        [MaxLength(MaxHouseholdEmailLength)]
        public string? Email { get; set; }
    }
}
