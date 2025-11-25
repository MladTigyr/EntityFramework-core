namespace Trucks.DataProcessor.ImportDto.Xml
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Common.EntityValidation;

    [XmlType("Despatcher")]
    public class ImportDespatcherDto
    {
        [Required]
        [XmlElement("Name")]
        [MinLength(MinDespatcherNameLength)]
        [MaxLength(MaxDespatcherNameLength)]
        public string Name { get; set; } = null!;

        [XmlElement("Position")]
        public string? Position { get; set; }

        [Required]
        [XmlArray("Trucks")]
        public ImportTrucksDto[] Truck { get; set; } = null!;
    }
}
