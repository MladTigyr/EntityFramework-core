namespace NetPay.Common
{
    public static class EntityValidation
    {
        //Household
        public const int MinHouseholdContactPersonLength = 5;
        public const int MaxHouseholdContactPersonLength = 50;
        public const int MinHouseholdEmailLength = 6;
        public const int MaxHouseholdEmailLength = 80;
        public const string HouseholdRegex = @"^\+\d{3}/\d{3}-\d{6}$";

        //Expense
        public const int MinExpenseNameLength = 5;
        public const int MaxExpenseNameLength = 50;
        public const decimal MinExpenseAmountValue = 0.01m;
        public const decimal MaxExpenseAmountValue = 100000.00m;
        public const int MinExpenseEnumValue = 1;
        public const int MaxExpenseEnumValue = 4;

        //Service
        public const int MinServiceNameLength = 5;
        public const int MaxServiceNameLength = 30;

        //Supplier
        public const int MinSupplierNameLength = 3;
        public const int MaxSupplierNameLength = 60;
    }
}
