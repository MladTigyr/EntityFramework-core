using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Common
{
    public static class EntityValidation
    {
        public static class Patient
        {
            public const int FirstNameMaxLength = 50;
            public const int LastNameMaxLength = 50;
            public const int AddressMaxLength = 250;
            public const int EmailMaxLength = 80;
        }

        public static class Visitation
        {
            public const int CommentMaxLength = 250;
        }

        public static class Diagnose
        {
            public const int NameMaxLength = 50;
            public const int CommentMaxLength = 250;
        }

        public static class Medicament
        {
            public const int NameMaxLength = 50;
        }

        public static class Doctor
        {
            public const int NameMaxLength = 100;

            public const int SpecialtyMaxLength = 100;
        }
    }
}
