using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataValidation
{
    public static class DataValidation
    {
        public static class District
        {
            public const int MinNameLength = 2;
            public const int MaxNameLength = 80;
            public const int PostalLength = 8;
            public const string DistrictPostalCodeRegex = @"^[A-Z]{2}-\d{5}$";
        }

        public static class Property
        {
            public const int MinPropertyIdentifierLength = 16;
            public const int MaxPropertyIdentifierLength = 20;
            public const int MinDetailsLength = 5;
            public const int MaxDetailsLength = 500;
            public const int MinAddressLength = 2;
            public const int MaxAddressLength = 200;
        }

        public static class Citizen
        {
            public const int MinFirstNameLength = 2;
            public const int MaxFirstNameLength = 30;
            public const int MinLastNameLength = 2;
            public const int MaxLastNameLength = 30;
        }
    }
}
