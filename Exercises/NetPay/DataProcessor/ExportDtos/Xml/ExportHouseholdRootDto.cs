namespace NetPay.DataProcessor.ExportDtos.Xml
{
    using System.Xml.Serialization;

    [XmlRoot("Households")]
    public class ExportHouseholdRootDto
    {
        [XmlElement("Household")]
        public ExportHouseholdDetailsDto[] Households { get; set; } = null!;
    }
}
