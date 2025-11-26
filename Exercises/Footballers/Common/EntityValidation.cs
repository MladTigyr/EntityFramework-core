namespace Footballers.Common
{
    public class EntityValidation
    {
        //Footballer
        public const int MinFootballerNameLength = 2;
        public const int MaxFootballerNameLength = 40;
        public const int MinFootballerPositionValue = 0;
        public const int MaxFootballerPositionValue = 3;
        public const int MinFootballerSkillValue = 0;
        public const int MaxFootballerSkillValue = 4;

        //Team
        public const int MinTeamNameLength = 3;
        public const int MaxTeamNameLength = 40;
        public const string TeamRegex = "^[A-Za-z0-9.\\- ]+$";
        public const int MinTeamNationalityLength = 2;
        public const int MaxTeamNationalityLength = 40;

        //Coach
        public const int MinCoachNameLength = 3;
        public const int MaxCoachNameLength = 40;
    }
}
