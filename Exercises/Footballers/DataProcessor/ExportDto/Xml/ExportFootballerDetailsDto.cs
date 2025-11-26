namespace Footballers.DataProcessor.ExportDto.Xml
{
    using System.Xml.Serialization;

    [XmlType("Footballer")]
    public class ExportFootballerDetailsDto
    {
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("Position")]
        public string Position { get; set; } = null!;
    }
}