namespace Theatre.DataProcessor.ExportDto.Xml
{
    using System.Xml.Serialization;

    [XmlRoot("Plays")]
    public class ExportPlayRootDto
    {
        [XmlElement("Play")]
        public ExportPlayDetailsDto[] Plays { get; set; } = null!;
    }
}
