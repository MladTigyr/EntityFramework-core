namespace Trucks.DataProcessor.ImportDto.Xml
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Common.EntityValidation;

    [XmlType("Truck")]
    public class ImportTrucksDto
    {
        [Required]
        [XmlElement("RegistrationNumber")]
        [RegularExpression(TruckRegex)]
        public string RegistrationNumber { get; set; } = null!;

        [Required]
        [XmlElement("VinNumber")]
        [MinLength(TruckVinNumberLength)]
        [MaxLength(TruckVinNumberLength)]
        public string VinNumber { get; set; } = null!;

        [XmlElement("TankCapacity")]
        [Range(MinTruckTankCapacityValue, MaxTruckTankCapacityValue)]
        public int TankCapacity { get; set; }

        [XmlElement("CargoCapacity")]
        [Range(MinTruckCargoCapacityValue, MaxTruckCargoCapacityValue)]
        public int CargoCapacity { get; set; }

        [XmlElement("CategoryType")]
        [Range(MinTruckCategoryTypeValue, MaxTruckCategoryTypeValue)]
        public int CategoryType { get; set; }

        [XmlElement("MakeType")]
        [Range(MinTruckMakeTypeValue, MaxTruckMakeTypeValue)]
        public int MakeType { get; set; }
    }
}