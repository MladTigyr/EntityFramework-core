namespace Theatre.DataProcessor
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto.Json;
    using Theatre.DataProcessor.ImportDto.Xml;
    using Theatre.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Play> plays = new List<Play>();

            IEnumerable<ImportPlayDto>? importPlayDtos = XmlSerializeWrapper
                .Deserialize<ImportPlayDto[]>(xmlString, "Plays");

            if (importPlayDtos != null)
            {
                foreach (ImportPlayDto playDto in importPlayDtos)
                {
                    if (!IsValid(playDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    TimeSpan currTimeSpan = TimeSpan
                        .ParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture);

                    if (currTimeSpan < TimeSpan.FromHours(1))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isGenreValid = Enum
                        .TryParse<Genre>(playDto.Genre, out Genre genreValue);

                    if (!isGenreValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Play play = new Play()
                    {
                        Title = playDto.Title,
                        Duration = currTimeSpan,
                        Rating = (float)playDto.Rating,
                        Genre = genreValue,
                        Description = playDto.Description,
                        Screenwriter = playDto.Screewriter,
                    };

                    plays.Add(play);

                    sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre, play.Rating));
                }

                context.Plays.AddRange(plays);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Cast> casts = new List<Cast>();

            IEnumerable<ImportCastDto>? importCastDtos = XmlSerializeWrapper
                .Deserialize<ImportCastDto[]>(xmlString, "Casts");

            if (importCastDtos != null)
            {
                foreach (ImportCastDto castDto in importCastDtos)
                {
                    if (!IsValid(castDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Cast cast = new Cast()
                    {
                        FullName = castDto.FullName,
                        IsMainCharacter = bool.Parse(castDto.IsMainCharacter),
                        PhoneNumber = castDto.PhoneNumber,
                        PlayId = castDto.PlayId,
                    };

                    casts.Add(cast);

                    sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, cast.IsMainCharacter == true ? "main" : "lesser"));
                }

                context.Casts.AddRange(casts);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Theatre> theatres = new List<Theatre>();

            IEnumerable<ImportTheatreDto>? importTheatreDtos = JsonConvert
                .DeserializeObject<ImportTheatreDto[]>(jsonString);

            if (importTheatreDtos != null)
            {
                foreach (ImportTheatreDto theatreDto in importTheatreDtos)
                {
                    if (!IsValid(theatreDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Theatre theatre = new Theatre() 
                    {
                        Name = theatreDto.Name,
                        NumberOfHalls = theatreDto.NumberOfHalls,
                        Director = theatreDto.Director,
                    };

                    ICollection<Ticket> tickets = new List<Ticket>();

                    foreach (ImportTicketDto ticket in theatreDto.Tickets)
                    {
                        if (!IsValid(ticket))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        Ticket currTicket = new Ticket()
                        {
                            Price = ticket.Price,
                            RowNumber = ticket.RowNumber,
                            PlayId = ticket.PlayId,
                        };

                        tickets.Add(currTicket);
                    }
                    theatre.Tickets = tickets;
                    theatres.Add(theatre);

                    sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
                }

                context.Theatres.AddRange(theatres);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
