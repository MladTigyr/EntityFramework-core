namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto.Xml;
    using Boardgames.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Xml.Linq;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            ExportCreatorsRootDto exportDtos = new ExportCreatorsRootDto()
            {
                Creator = context.Creators
                    .AsNoTracking()
                    .Select(c => new ExportCreatorDetailsDto
                    {
                        BoardgamesCount = c.Boardgames.Count,
                        CreatorName = c.FirstName + " " + c.LastName,
                        Boardgame = c.Boardgames
                            .Select(b => new ExportBoardgameDetailsDto
                            {
                                BoardgameName = b.Name,
                                BoardgameYearPublished = b.YearPublished,
                            })
                            .OrderBy(b => b.BoardgameName)
                            .ToArray()
                    })
                    .OrderByDescending(c => c.BoardgamesCount)
                    .ThenBy(c => c.CreatorName)
                    .ToArray()
            };

            string result = XmlSerializeWrapper
                .Serialize(exportDtos, "Creators");

            return result;
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .Where(s => s.BoardgamesSellers.Any(b => b.Boardgame.YearPublished >= year &&
                                                        b.Boardgame.Rating <= rating))
                .ToArray()
                .Select(s => new
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers
                        .Where(b => b.Boardgame.YearPublished >= year &&
                                    b.Boardgame.Rating <= rating)
                        .ToArray()
                        .OrderByDescending(b => b.Boardgame.Rating)
                        .ThenBy(b => b.Boardgame.Name)
                        .Select(b => new
                        {
                            Name = b.Boardgame.Name,
                            Rating = b.Boardgame.Rating,
                            Mechanics = b.Boardgame.Mechanics,
                            Category = b.Boardgame.CategoryType.ToString(),
                        })
                        .ToArray()
                })
                .OrderByDescending(s => s.Boardgames.Count())
                .ThenBy(s => s.Name)
                .Take(5)
                .ToArray();

            string result = JsonConvert
                .SerializeObject(sellers, Formatting.Indented);

            return result;
        }
    }
}