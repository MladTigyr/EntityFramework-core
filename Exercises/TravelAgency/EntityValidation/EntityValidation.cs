using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.EntityValidation
{
    public static class EntityValidation
    {
        public const int MinCustomerFullNameLength = 4;
        public const int MaxCustomerFullNameLength = 60;
        public const int MinCustomerEmailLength = 6;
        public const int MaxCustomerEmailLength = 50;
        public const int CustomerPhoneNumberLength = 13;
        public const string CustomerPhoneNumber = "\\+[3][5][9][8][789]\\d{7}";

        public const int MinGuideFullNameLength = 4;
        public const int MaxGuideFullNameLength = 60;

        public const int MinPackageNameLength = 2;
        public const int MaxPackageNameLength = 40;
        public const int MaxPackageDescriptionLength = 200;
    }
}
