namespace Medicines.DataProcessor.ExportDtos.Xml
{
    using System.Xml.Serialization;


    [XmlRoot("Patients")]
    public class ExportPatientsRootDto
    {
        [XmlElement("Patient")]
        public ExportPatientDetailsDto[] Patient { get; set; } = null!;
    }
}
