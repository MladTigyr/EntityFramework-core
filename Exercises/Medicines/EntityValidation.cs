using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines
{
    public static class EntityValidation
    {
        public const int MinPharmacyNameLength = 2;
        public const int MaxPharmacyNameLength = 50;
        public const string PhoneNumberRegEx = "\\([0-9]{3}\\) [0-9]{3}\\-[0-9]{4}";

        public const int MinMedicineNameLength = 3;
        public const int MaxMedicineNameLength = 150;
        public const int MinProducerMedicineLength = 3;
        public const int MaxProducerMedicineLength = 100;
        public const decimal MinMedicinePrice = 0.01m;
        public const decimal MaxMedicinePrice = 1000.0m;
        public const int MinCategoryValue = 0;
        public const int MaxCategoryValue = 4;

        public const int MinPatientFullNameLength = 5;
        public const int MaxPatientFullNameLength = 100;
        public const int MinAgeValue = 0;
        public const int MaxAgeValue = 2;
        public const int MinGenderValue = 0;
        public const int MaxGenderValue = 1;

    }
}
