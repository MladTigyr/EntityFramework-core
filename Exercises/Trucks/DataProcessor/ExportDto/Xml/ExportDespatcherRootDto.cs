namespace Trucks.DataProcessor.ExportDto.Xml
{
    using System.Xml.Serialization;

    [XmlRoot("Despatchers")]
    public class ExportDespatcherRootDto
    {
        [XmlElement("Despatcher")]
        public ExportDespatcherDetailsDto[] Despatcher { get; set; } = null!;
    }
}
