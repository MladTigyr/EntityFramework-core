namespace Trucks.Common
{
    public static class EntityValidation
    {
        //Truck
        public const string TruckRegex = "^[A-Z]{2}[0-9]{4}[A-Z]{2}$";
        public const int TruckVinNumberLength = 17;
        public const int MinTruckTankCapacityValue = 950;
        public const int MaxTruckTankCapacityValue = 1420;
        public const int MinTruckCargoCapacityValue = 5000;
        public const int MaxTruckCargoCapacityValue = 29000;
        public const int MinTruckCategoryTypeValue = 0;
        public const int MaxTruckCategoryTypeValue = 3;
        public const int MinTruckMakeTypeValue = 0;
        public const int MaxTruckMakeTypeValue = 4;

        //Client
        public const int MinClientNameLength = 3;
        public const int MaxClientNameLength = 40;
        public const int MinClientNationalityLength = 2;
        public const int MaxClientNationalityLength = 40;

        //Despatcher
        public const int MinDespatcherNameLength = 2;
        public const int MaxDespatcherNameLength = 40;
    }
}
