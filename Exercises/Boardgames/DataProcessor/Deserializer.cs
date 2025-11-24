namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto.Json;
    using Boardgames.DataProcessor.ImportDto.Xml;
    using Boardgames.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Creator> creators = new List<Creator>();

            IEnumerable<ImportCreatorDetailsDto>? importCreatorDetailsDtos = XmlSerializeWrapper
                .Deserialize<ImportCreatorDetailsDto[]>(xmlString, "Creators");

            if (importCreatorDetailsDtos != null)
            {
                foreach (ImportCreatorDetailsDto creatorDto in importCreatorDetailsDtos)
                {
                    if (!IsValid(creatorDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Creator creator = new Creator()
                    {
                        FirstName = creatorDto.FirstName,
                        LastName = creatorDto.LastName,
                    };

                    ICollection<Boardgame> boardgames = new List<Boardgame>();

                    foreach (ImportBoardgamesDetailsDto boardGameDto in creatorDto.Boardgames)
                    {
                        if (!IsValid(boardGameDto))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        Boardgame boardgame = new Boardgame()
                        {
                            Name = boardGameDto.Name,
                            Rating = boardGameDto.Rating,
                            YearPublished = boardGameDto.YearPublished,
                            CategoryType = (CategoryType)boardGameDto.CategoryType,
                            Mechanics = boardGameDto.Mechanics,
                            Creator = creator
                        };

                        boardgames.Add(boardgame);
                    }

                    creator.Boardgames = boardgames;
                    creators.Add(creator);

                    sb.AppendLine(string.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count));
                }

                context.Creators.AddRange(creators);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Seller> sellers = new List<Seller>();
            ICollection<int> boardGamesIds = context.Boardgames
                .AsNoTracking()
                .Select(x => x.Id)
                .ToArray();

            IEnumerable<ImportSellerDto>? importSellerDtos = JsonConvert
                .DeserializeObject<ImportSellerDto[]>(jsonString);

            if (importSellerDtos != null)
            {
                foreach (ImportSellerDto sellerDto in importSellerDtos)
                {
                    if (!IsValid(sellerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Seller seller = new Seller()
                    {
                        Name = sellerDto.Name,
                        Address = sellerDto.Address,
                        Country = sellerDto.Country,
                        Website = sellerDto.Website,
                    };

                    ICollection<int> boardgamesUniqueIds = new HashSet<int>();

                    ICollection<BoardgameSeller> boardgameSellers = new List<BoardgameSeller>();

                    foreach (int currId in sellerDto.Boardgames)
                    {
                        boardgamesUniqueIds.Add(currId);
                    }

                    foreach (int currId in boardgamesUniqueIds)
                    {
                        if (!boardGamesIds.Contains(currId))
                        {
                            sb.AppendLine(ErrorMessage);
                            boardgamesUniqueIds.Remove(currId);
                            continue;
                        }

                        BoardgameSeller boardgameSeller = new BoardgameSeller()
                        {
                            BoardgameId = currId,
                            Seller = seller,
                        };

                        boardgameSellers.Add(boardgameSeller);
                    }

                    seller.BoardgamesSellers = boardgameSellers;
                    sellers.Add(seller);

                    sb.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count));
                }

                context.Sellers.AddRange(sellers);
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
