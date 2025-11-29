using System.Xml.Serialization;

namespace NetPay.DataProcessor.ExportDtos.Xml
{
    [XmlType("Household")]
    public class ExportHouseholdDetailsDto
    {
        [XmlElement("ContactPerson")]
        public string ContactPerson { get; set; } = null!;

        [XmlElement("Email")]
        public string? Email { get; set; }

        [XmlElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = null!;

        [XmlArray("Expenses")]
        public ExportExpenseDetailsDto[] Expenses { get; set; } = null!;
    }
}