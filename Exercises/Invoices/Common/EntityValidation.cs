using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Common
{
    public static class EntityValidation
    {
        //Product
        public const int MinProductNameLength = 9;
        public const int MaxProductNameLength = 30;
        public const decimal MinProductPriceRange = 5.00m;
        public const decimal MaxProductPriceRange = 1000.00m;
        public const int MinProductEnumRange = 0;
        public const int MaxProductEnumRange = 4;

        //Address
        public const int MinAddressStreetNameLength = 10;
        public const int MaxAddressStreetNameLength = 20;
        public const int MinAddressCityLength = 5;
        public const int MaxAddressCityLength = 15;
        public const int MinAddressCountryLength = 5;
        public const int MaxAddressCountryLength = 15;

        //Invoice
        public const int MinInvoiceNumberRange = 1000000000;
        public const int MaxInvoiceNumberRange = 1500000000;
        public const int MinInvoiceEnumRange = 0;
        public const int MaxInvoiceEnumRange = 2;

        //Client
        public const int MinClientNameLength = 10;
        public const int MaxClientNameLength = 25;
        public const int MinClientNumberVatLength = 10;
        public const int MaxClientNumberVatLength = 25;
    }
}
