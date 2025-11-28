namespace Theatre.DataProcessor.ExportDto.Xml
{
    using System.Xml.Serialization;

    [XmlType("Actor")]
    public class ExportActorDetailsDto
    {
        [XmlAttribute("FullName")]
        public string FullName { get; set; } = null!;

        [XmlAttribute("MainCharacter")]
        public string MainCharacter { get; set; } = null!;
    }
}