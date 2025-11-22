namespace Medicines.DataProcessor.ImportDtos.Xml
{
    using Medicines.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using static EntityValidation;

    [XmlType("Medicine")]
    public class ImportMedicinesDetailsDto
    {
        [Required]
        [XmlAttribute("category")]
        [Range(MinCategoryValue, MaxCategoryValue)]
        public int Category { get; set; }

        [Required]
        [XmlElement("Name")]
        [MinLength(MinMedicineNameLength)]
        [MaxLength(MaxMedicineNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("Price")]
        [Range((double)MinMedicinePrice, (double)MaxMedicinePrice)]
        public decimal Price { get; set; }

        [Required]
        [XmlElement("ProductionDate")]
        public string ProductionDate { get; set; } = null!;

        [Required]
        [XmlElement("ExpiryDate")]
        public string ExpiryDate { get; set; } = null!;

        [Required]
        [XmlElement("Producer")]
        [MinLength(MinProducerMedicineLength)]
        [MaxLength(MaxProducerMedicineLength)]
        public string Producer { get; set; } = null!;
    }
}