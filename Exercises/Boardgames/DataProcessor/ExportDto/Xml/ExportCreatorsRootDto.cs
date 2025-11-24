namespace Boardgames.DataProcessor.ExportDto.Xml
{
    using System.Xml.Serialization;

    [XmlRoot("Creators")]
    public class ExportCreatorsRootDto
    {
        [XmlElement("Creator")]
        public ExportCreatorDetailsDto[] Creator { get; set; } = null!;
    }
}
