using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_SalesDatabase.Common
{
    public static class EntityValidation
    {
        public class Product
        {
            public const int NameMaxLength = 50;
        }
        public class Customer 
        {
            public const int NameMaxLength = 100;
            public const int EmailMaxLength = 80;
        }

        public class Store
        {
            public const int NameMaxLength = 80;
        }
    }
}
