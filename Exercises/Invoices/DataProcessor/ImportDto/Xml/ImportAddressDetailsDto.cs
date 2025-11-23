namespace Invoices.DataProcessor.ImportDto.Xml
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using static Common.EntityValidation;

    [XmlType("Address")]
    public class ImportAddressDetailsDto
    {
        [Required]
        [XmlElement("StreetName")]
        [MinLength(MinAddressStreetNameLength)]
        [MaxLength(MaxAddressStreetNameLength)]
        public string StreetName { get; set; } = null!;

        [Required]
        [XmlElement("StreetNumber")]
        public int StreetNumber { get; set; }

        [Required]
        [XmlElement("PostCode")]
        [MinLength(1)]
        public string PostCode { get; set; } = null!;

        [Required]
        [XmlElement("City")]
        [MinLength(MinAddressCityLength)]
        [MaxLength(MaxAddressCityLength)]
        public string City { get; set; } = null!;

        [Required]
        [XmlElement("Country")]
        [MinLength(MinAddressCountryLength)]
        [MaxLength(MaxAddressCountryLength)]
        public string Country { get; set; } = null!;
    }
}