namespace Artillery.Common
{
    public static class EntityValidation
    {
        //Country
        public const int MinCountryNameLength = 4;
        public const int MaxCountryNameLength = 60;
        public const int MinCountryArmySize = 50000;
        public const int MaxCountryArmySize = 10000000;

        //Manufacturer
        public const int MinManufacturerNameLength = 4;
        public const int MaxManufacturerNameLength = 40;
        public const int MinManufacturerFounded = 10;
        public const int MaxManufacturerFounded = 100;

        //Shell
        public const double MinShellWeight = 2.0;
        public const double MaxShellWeight = 1680.0;
        public const int MinShellCaliberLength = 4;
        public const int MaxShellCaliberLength = 30;

        //Gun
        public const int MinGunWeight = 100;
        public const int MaxGunWeight = 1350000;
        public const double MinGunBarrelLength = 2.00;
        public const double MaxGunBarrelLength = 35.00;
        public const int MinGunRange = 1;
        public const int MaxGunRange = 100000;
        public const int MinGunType = 0;
        public const int MaxGunType = 5;
    }
}
