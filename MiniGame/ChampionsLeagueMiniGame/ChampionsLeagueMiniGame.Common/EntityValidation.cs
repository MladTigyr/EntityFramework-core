using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChampionsLeagueMiniGame.Common
{
    public static class EntityValidation
    {
        public static class Team
        {
            public const int NameMaxLength = 50;
        }

        public static class Player
        {
            public const int FirstNameMaxLength = 50;
            public const int LastNameMaxLength = 50;
        }

        public static class Nationality
        {
            public const int NationalityMaxLength = 56;
        }
    }
}
