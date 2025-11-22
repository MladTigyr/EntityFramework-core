
namespace Medicines.DataProcessor.ExportDtos.Xml
{
    using Medicines.Data.Models.Enums;
    using System.Xml.Serialization;


    [XmlType("Medicine")]
    public class ExportMedicineDetailsDto
    {
        [XmlAttribute("Category")]
        public string Category { get; set; } = null!;

        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("Price")]
        public string Price { get; set; } = null!;

        [XmlElement("Producer")]
        public string Producer { get; set; } = null!;

        [XmlElement("BestBefore")]
        public string BestBefore { get; set; } = null!;
    }
}