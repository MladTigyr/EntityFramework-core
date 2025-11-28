namespace Artillery.DataProcessor.ExportDto.Xml
{
    using System.Xml.Serialization;

    [XmlRoot("Guns")]
    public class ExportGunRootDto
    {
        [XmlElement("Gun")]
        public ExportGunDetailsDto[] Guns { get; set; } = null!;
    }
}
