namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos.Xml;
    using Medicines.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            DateTime dateToCompare = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var patients = context.Patients
                .AsNoTracking()
                .Where(p => p.PatientsMedicines.Any(pm => pm.Medicine != null && pm.Medicine.ProductionDate > dateToCompare))
                .Select(p => new 
                {
                    Patient = new 
                    {
                        Gender = p.Gender,
                        Name = p.FullName,
                        AgeGroup = p.AgeGroup,
                        Medicine = p.PatientsMedicines
                            .Where(pm => pm.Medicine != null && pm.Medicine.ProductionDate > dateToCompare)
                            .Select(pm => new 
                            {
                                Category = pm.Medicine.Category,
                                Name = pm.Medicine.Name,
                                Price = pm.Medicine.Price,
                                Producer = pm.Medicine.Producer,
                                BestBefore = pm.Medicine.ExpiryDate,
                            })
                            .OrderByDescending(m => m.BestBefore)
                            .ThenBy(m => m.Price)
                            .ToArray()
                    }
                })
                .OrderByDescending(p => p.Patient.Medicine.Count())
                .ThenBy(p => p.Patient.Name)
                .ToArray();

            ExportPatientsRootDto exportDto = new ExportPatientsRootDto
            {
                Patient = patients
                    .Select(p => new ExportPatientDetailsDto
                    {
                        Gender = p.Patient.Gender.ToString().ToLower(),
                        Name = p.Patient.Name,
                        AgeGroup = p.Patient.AgeGroup,
                        Medicine = p.Patient.Medicine
                            .Select(pm => new ExportMedicineDetailsDto
                            {
                                Category = pm.Category.ToString().ToLower(),
                                Name = pm.Name,
                                Price = pm.Price.ToString("F2"),
                                Producer = pm.Producer,
                                BestBefore = pm.BestBefore.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                            })
                            .ToArray()
                    })
                    .ToArray()
            };

            string result = XmlSerializeWrapper
                .Serialize(exportDto, "Patients");

            return result;
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var medicines = context.Medicines
                .AsNoTracking()
                .Where(m => m.Category == (Category)medicineCategory && m.Pharmacy.IsNonStop == true)
                .Select(m => new
                {
                    Name = m.Name,
                    Price = m.Price,
                    Pharmacy = new
                    {
                        Name = m.Pharmacy.Name,
                        PhoneNumber = m.Pharmacy.PhoneNumber,
                    }

                })
                .OrderBy(m => m.Price)
                .ThenBy(m => m.Name)
                .ToArray();

            var exportDtos = medicines
                .Select(m => new
                {
                    Name = m.Name,
                    Price = m.Price.ToString("F2"),
                    Pharmacy = new
                    {
                        Name = m.Pharmacy.Name,
                        PhoneNumber = m.Pharmacy.PhoneNumber,
                    }
                })
                .ToArray();

            string result = JsonConvert.SerializeObject(exportDtos, Formatting.Indented);

            return result;
        }
    }
}
