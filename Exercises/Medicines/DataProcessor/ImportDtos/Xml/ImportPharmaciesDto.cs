namespace Medicines.DataProcessor.ImportDtos.Xml
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static EntityValidation;

    [XmlType("Pharmacy")]
    public class ImportPharmaciesDto
    {
        [Required]
        [XmlAttribute("non-stop")]
        public string NonStop { get; set; } = null!;

        [Required]
        [XmlElement("Name")]
        [MinLength(MinPharmacyNameLength)]
        [MaxLength(MaxPharmacyNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("PhoneNumber")]
        [RegularExpression(PhoneNumberRegEx)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [XmlArray("Medicines")]
        public ImportMedicinesDetailsDto[] Medicine { get; set; } = null!;
    }
}
