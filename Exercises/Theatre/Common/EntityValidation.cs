namespace Theatre.Common
{
    public static class EntityValidation
    {
        //Theatre
        public const int MinTheatreNameLength = 4;
        public const int MaxTheatreNameLength = 30;
        public const sbyte MinTheatreNumberOfHalls = 1;
        public const sbyte MaxTheatreNumberOfHalls = 10;
        public const int MinTheatreDirectorLength = 4;
        public const int MaxTheatreDirectorLength = 30;

        //Play
        public const int MinPlayTitleLength = 4;
        public const int MaxPlayTitleLength = 50;
        public static readonly TimeSpan PlayDuration = TimeSpan.FromHours(1);
        public const float MinPlayRatinValue = 0.0f;
        public const float MaxPlayRatinValue = 10.0f;
        public const int MinPlayEnum = 0;
        public const int MaxPlayEnum = 3;
        public const int MinPlayDescriptionLength = 0;
        public const int MaxPlayDescriptionLength = 700;
        public const int MinPlayScreeWriterLength = 4;
        public const int MaxPlayScreeWriterLength = 30;

        //Cast
        public const int MinCastNameLength = 4;
        public const int MaxCastNameLength = 30;
        public const string CastRegex = "^\\+[4]{2}\\-[0-9]{2}\\-[0-9]{3}\\-[0-9]{4}$";

        //Ticket
        public const decimal MinTicketPriceValue = 1.0m;
        public const decimal MaxTicketPriceValue = 100.0m;
        public const sbyte MinTickerRowNum = 1;
        public const sbyte MaxTickerRowNum = 10;
    }
}
