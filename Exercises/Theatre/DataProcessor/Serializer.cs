namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics;
    using System.Xml.Linq;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto.Xml;
    using Theatre.Utilities;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var dtos = context.Theatres
                .Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count >= 20)
                .ToArray()
                .Select(t => new
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets
                        .Where(ti => ti.RowNumber >= 1 && ti.RowNumber <= 5)
                        .Sum(ti => ti.Price),
                    Tickets = t.Tickets
                        .Where(ti => ti.RowNumber >= 1 && ti.RowNumber <= 5)
                        .ToArray()
                        .Select(ti => new
                        {
                            Price = ti.Price,
                            RowNumber = ti.RowNumber
                        })
                        .OrderByDescending(ti => ti.Price)
                        .ToArray()
                })
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

            string result = JsonConvert
                .SerializeObject(dtos, Formatting.Indented);

            return result;
        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {
            ExportPlayRootDto dtos = new ExportPlayRootDto
            {
                Plays = context.Plays
                    .Where(p => p.Rating <= raiting)
                    .ToArray()
                    .Select(p => new ExportPlayDetailsDto
                    {
                        Title = p.Title,
                        Duration = p.Duration.ToString("c"),
                        Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                        Genre = p.Genre.ToString(),
                        Actors = p.Casts
                            .Where(c => c.IsMainCharacter)
                            .ToArray()
                            .Select(c => new ExportActorDetailsDto
                            {
                                FullName = c.FullName,
                                MainCharacter = $"Plays main character in '{c.Play.Title}'."
                            })
                            .OrderByDescending(c => c.FullName)
                            .ToArray()
                    })
                    .OrderBy(p => p.Title)
                    .ThenByDescending(p => p.Genre)
                    .ToArray()
            };

            string result = XmlSerializeWrapper
                .Serialize(dtos, "Plays");

            return result;
        }
    }
}
