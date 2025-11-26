namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ExportDto.Xml;
    using Footballers.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Xml.Linq;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coaches = new
            {
                Coach = context.Coaches
                        .AsNoTracking()
                        .Where(c => c.Footballers.Any())
                        .Select(c => new
                        {
                            FootballersCount = c.Footballers.Count,
                            CoachName = c.Name,
                            Footballers = c.Footballers
                                .Select(f => new
                                {
                                    Name = f.Name,
                                    Position = f.PositionType
                                })
                                .OrderBy(f => f.Name)
                                .ToArray()
                        })
                        .OrderByDescending(c => c.FootballersCount)
                        .ThenBy(c => c.CoachName)
                        .ToArray()
            };

            ExportCoachRootDto exportDtos = new ExportCoachRootDto
            {
                Coaches = coaches.Coach
                    .Select(c => new ExportCoachDetailsDto
                    {
                        FootballersCount = c.FootballersCount,
                        CoachName = c.CoachName,
                        Footballers = c.Footballers
                            .Select(f => new ExportFootballerDetailsDto
                            {
                                Name = f.Name,
                                Position = f.Position.ToString()
                            })
                            .ToArray()
                    })
                    .ToArray()
            };

            string result = XmlSerializeWrapper
                .Serialize(exportDtos, "Coaches");

            return result;
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .ToArray()
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                        .Where(f => f.Footballer.ContractStartDate >= date)
                        .ToArray()
                        .OrderByDescending(f => f.Footballer.ContractEndDate)
                        .ThenBy(f => f.Footballer.Name)
                        .Select(f => new
                        {
                            FootballerName = f.Footballer.Name,
                            ContractStartDate = f.Footballer.ContractStartDate.ToString("d"),
                            ContractEndDate = f.Footballer.ContractEndDate.ToString("d"),
                            BestSkillType = f.Footballer.BestSkillType.ToString(),
                            PositionType = f.Footballer.PositionType.ToString(),
                        })
                        .ToArray()
                })
                .OrderByDescending(t => t.Footballers.Count())
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();

            string result = JsonConvert
                .SerializeObject(teams, Formatting.Indented);

            return result;
        }
    }
}
