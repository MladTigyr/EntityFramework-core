namespace Trucks.DataProcessor.ExportDto.Xml
{
    using System.Xml.Serialization;

    [XmlType("Truck")]
    public class ExportTruckDetailsDto
    {
        [XmlElement("RegistrationNumber")]
        public string RegistrationNumber { get; set; } = null!;

        [XmlElement("Make")]
        public string Make { get; set;} = null!;
    }
}