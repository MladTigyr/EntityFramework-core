namespace Medicines.DataProcessor.ExportDtos.Xml
{
    using Medicines.Data.Models.Enums;
    using System.Xml.Serialization;

    [XmlType("Patient")]
    public class ExportPatientDetailsDto
    {
        [XmlAttribute("Gender")]
        public string Gender { get; set; } = null!;

        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("AgeGroup")]
        public AgeGroup AgeGroup { get; set; }

        [XmlArray("Medicines")]
        public ExportMedicineDetailsDto[] Medicine { get; set; } = null!;
    }
}