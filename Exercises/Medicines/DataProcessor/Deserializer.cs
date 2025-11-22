namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos.Json;
    using Medicines.DataProcessor.ImportDtos.Xml;
    using Medicines.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Patient> patients = new List<Patient>();

            IEnumerable<ImportPatientsDto>? importPatientsDtos = JsonConvert
                .DeserializeObject<ImportPatientsDto[]>(jsonString);

            if (importPatientsDtos != null)
            {
                foreach (ImportPatientsDto patientDto in importPatientsDtos)
                {
                    if (!IsValid(patientDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Patient patient = new Patient()
                    {
                        FullName = patientDto.FullName,
                        AgeGroup = (AgeGroup)patientDto.AgeGroup,
                        Gender = (Gender)patientDto.Gender,
                    };

                    ICollection<int> patientIds = new List<int>();
                    ICollection<PatientMedicine> patientMedicines = new List<PatientMedicine>();

                    foreach (int medId in patientDto.Medicines)
                    {
                        if (patientIds.Contains(medId))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        patientIds.Add(medId);

                        PatientMedicine patientMedicine = new PatientMedicine()
                        {
                            MedicineId = medId,
                            Patient = patient,
                        };

                        patientMedicines.Add(patientMedicine);
                    }

                    patient.PatientsMedicines = patientMedicines;
                    patients.Add(patient);

                    sb.AppendLine(string.Format(SuccessfullyImportedPatient, patient.FullName, patient.PatientsMedicines.Count));
                }

                context.Patients.AddRange(patients);
                context.SaveChanges();
            }

            return sb.ToString();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Pharmacy> pharmacies = new List<Pharmacy>();

            IEnumerable<ImportPharmaciesDto>? importPharmaciesDto = XmlSerializeWrapper
                .Deserialize<ImportPharmaciesDto[]>(xmlString, "Pharmacies");

            if (importPharmaciesDto != null)
            {
                foreach (ImportPharmaciesDto pharmaciesDto in importPharmaciesDto)
                {
                    if (!IsValid(pharmaciesDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isValidNonStop = bool
                        .TryParse(pharmaciesDto.NonStop, out bool nonStopValue);

                    if (!isValidNonStop)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Pharmacy pharmacy = new Pharmacy()
                    {
                        Name = pharmaciesDto.Name,
                        PhoneNumber = pharmaciesDto.PhoneNumber,
                        IsNonStop = nonStopValue,
                    };

                    ICollection<Medicine> medicines = new List<Medicine>();

                    foreach (ImportMedicinesDetailsDto medicinesDetailsDto in pharmaciesDto.Medicine)
                    {
                        if (!IsValid(medicinesDetailsDto))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        DateTime productionDate = DateTime.ParseExact(medicinesDetailsDto.ProductionDate,
                            "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        DateTime expiryDate = DateTime.ParseExact(medicinesDetailsDto.ExpiryDate,
                            "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        if (productionDate >= expiryDate)
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        Medicine medicine = new Medicine()
                        {
                            Name = medicinesDetailsDto.Name,
                            Price = medicinesDetailsDto.Price,
                            Category = (Category)medicinesDetailsDto.Category,
                            ProductionDate = productionDate,
                            ExpiryDate = expiryDate,
                            Producer = medicinesDetailsDto.Producer,
                            Pharmacy = pharmacy
                        };

                        if (medicines.Any(m =>
                                m.Name == medicine.Name &&
                                m.Producer == medicine.Producer))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        medicines.Add(medicine);
                    }

                    pharmacy.Medicines = medicines;
                    pharmacies.Add(pharmacy);

                    sb.AppendLine(string.Format(SuccessfullyImportedPharmacy, pharmaciesDto.Name, pharmacy.Medicines.Count));
                }

                context.Pharmacies.AddRange(pharmacies);
                context.SaveChanges();

            }

            return sb.ToString();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
