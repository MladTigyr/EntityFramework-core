namespace Boardgames.Common
{
    public static class EntityValidation
    {
        //BoardGame
        public const int MinBoardGameNameLength = 10;
        public const int MaxBoardGameNameLength = 20;
        public const double MinBoardGameRatingValue = 1.0;
        public const double MaxBoardGameRatingValue = 10.0;
        public const int MinBoardGameYearPublished = 2018;
        public const int MaxBoardGameYearPublished = 2023;
        public const int MinBoardGameEnumValue = 0;
        public const int MaxBoardGameEnumValue = 4;

        //Seller
        public const int MinSellerNameLength = 5;
        public const int MaxSellerNameLength = 20;
        public const int MinSellerAddressLength = 2;
        public const int MaxSellerAddressLength = 30;
        public const string SellerWebsiteRegex = "^www\\.[A-Za-z0-9-]+\\.com$";

        //Creator
        public const int MinCreatorFirstNameLength = 2;
        public const int MaxCreatorFirstNameLength = 7;
        public const int MinCreatorLastNameLength = 2;
        public const int MaxCreatorLastNameLength = 7;
    }
}
