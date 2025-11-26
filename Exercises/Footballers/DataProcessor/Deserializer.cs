namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto.Json;
    using Footballers.DataProcessor.ImportDto.Xml;
    using Footballers.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Coach> coaches = new List<Coach>();

            IEnumerable<ImportCoachDto>? importCoachDtos = XmlSerializeWrapper
                .Deserialize<ImportCoachDto[]>(xmlString, "Coaches");

            if (importCoachDtos != null)
            {
                foreach (ImportCoachDto coachDto in importCoachDtos)
                {
                    if (!IsValid(coachDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (coachDto.Nationality == null || coachDto.Nationality.Length == 0)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Coach coach = new Coach()
                    {
                        Name = coachDto.Name,
                        Nationality = coachDto.Nationality,
                    };

                    ICollection<Footballer> footballers = new List<Footballer>();

                    foreach (ImportFootballerDto footballerDto in coachDto.Footballers)
                    {
                        if (!IsValid(footballerDto))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        bool isContractStartValid = DateTime
                            .TryParseExact(footballerDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out DateTime contractStartValue);

                        bool isContractEndValid = DateTime
                            .TryParseExact(footballerDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out DateTime contractEndValue);

                        if (!isContractStartValid || !isContractEndValid)
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        if (contractStartValue > contractEndValue) 
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        Footballer footballer = new Footballer()
                        {
                            Name = footballerDto.Name,
                            ContractStartDate = contractStartValue,
                            ContractEndDate = contractEndValue,
                            PositionType = (PositionType)footballerDto.PositionType,
                            BestSkillType = (BestSkillType)footballerDto.BestSkillType,
                            Coach = coach
                        };

                        footballers.Add(footballer);
                    }

                    coach.Footballers = footballers;
                    coaches.Add(coach);

                    sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
                }

                context.Coaches.AddRange(coaches);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Team> teams = new List<Team>();
            ICollection<int> footballersIds = context.Footballers
                .AsNoTracking()
                .Select(f => f.Id)
                .ToArray();

            IEnumerable<ImportTeamDto>? importTeamDtos = JsonConvert
                .DeserializeObject<ImportTeamDto[]>(jsonString);

            if (importTeamDtos != null)
            {
                foreach (ImportTeamDto teamDto in importTeamDtos)
                {
                    if (!IsValid(teamDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (teamDto.Trophies == 0)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Team team = new Team()
                    {
                        Name = teamDto.Name,
                        Nationality = teamDto.Nationality,
                        Trophies = teamDto.Trophies,
                    };

                    ICollection<int> uniqueIds = new HashSet<int>();

                    ICollection<TeamFootballer> teamFootballers = new HashSet<TeamFootballer>();

                    foreach (int currId in teamDto.Footballers)
                    {
                        uniqueIds.Add(currId);
                    }

                    foreach (int currId in uniqueIds)
                    {
                        if (!footballersIds.Contains(currId))
                        {
                            sb.AppendLine(ErrorMessage);
                            uniqueIds.Remove(currId);
                            continue;
                        }

                        TeamFootballer currTeamFootballer = new TeamFootballer()
                        {
                            Team = team,
                            FootballerId = currId,
                        };

                        teamFootballers.Add(currTeamFootballer);
                    }

                    team.TeamsFootballers = teamFootballers;
                    teams.Add(team);

                    sb.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
                }

                context.Teams.AddRange(teams);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
