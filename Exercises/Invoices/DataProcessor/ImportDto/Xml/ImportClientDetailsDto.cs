namespace Invoices.DataProcessor.ImportDto.Xml
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Common.EntityValidation;

    [XmlType("Client")]
    public class ImportClientDetailsDto
    {
        [Required]
        [XmlElement("Name")]
        [MinLength(MinClientNameLength)]
        [MaxLength(MaxClientNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("NumberVat")]
        [MinLength(MinClientNumberVatLength)]
        [MaxLength(MaxClientNumberVatLength)]
        public string NumberVat { get; set; } = null!;

        [Required]
        [XmlArray("Addresses")]
        public ImportAddressDetailsDto[] Address { get; set; } = null!;
    }
}
