using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.ExportCustomers
{
    [XmlType("customer")]
    public class ExportCustomerDetailDto
    {
        [XmlAttribute("full-name")]
        public string FullName { get; set; } = null!;

        [XmlAttribute("bought-cars")]
        public int BoughtCars { get; set; }

        [XmlAttribute("spent-money")]
        public decimal SpentMoney { get; set; } 
    }
}