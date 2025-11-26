namespace Footballers.DataProcessor.ExportDto.Xml
{
    using System.Xml.Serialization;

    [XmlRoot("Coaches")]
    public class ExportCoachRootDto
    {
        [XmlElement("Coach")]
        public ExportCoachDetailsDto[] Coaches { get; set; } = null!;
    }
}
