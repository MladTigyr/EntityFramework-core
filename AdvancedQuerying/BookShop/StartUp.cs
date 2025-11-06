namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //string input = Console.ReadLine();

            //if (input != null)
            //{
            //    string result = GetBooksByAuthor(db, input);

            //    Console.WriteLine(result);
            //}

            string result = GetMostRecentBooks(db);
            Console.WriteLine(result);

        }

        //Problem 2
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();

            bool isType = Enum.TryParse(command, true, out AgeRestriction restriction);

            if (isType)
            {
                var books = context.Books
                .Where(b => b.AgeRestriction == restriction)
                .AsNoTracking()
                .Select(b => new
                {
                    b.Title
                })
                .OrderBy(b => b.Title)
                .ToArray();

                foreach (var book in books)
                {
                    sb.AppendLine(book.Title);
                }
            }

            return sb.ToString().Trim();
        }

        //Problem 3
        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            EditionType editionType = EditionType.Gold;

            IEnumerable<string> books = context.Books
                .Where(b => b.EditionType == editionType && b.Copies < 5000)
                .AsNoTracking()
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().Trim();
        }

        //Problem 4
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var books = context.Books
                .Where (b => b.Price > 40)
                .AsNoTracking()
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToArray();

            foreach (var book in books)
            {
                stringBuilder.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return stringBuilder.ToString().Trim();
        }

        //Problem 5
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb = new();

            IEnumerable<string> books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .AsNoTracking()
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().Trim();
        }

        //Problem 6
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            string[] categoriesArr = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLowerInvariant())
                .ToArray();

            if (categoriesArr.Length > 1)
            {
                IEnumerable<string> books = context.Books
                .AsNoTracking()
                .Where(b => b.BookCategories
                    .Select(bc => bc.Category)
                    .Any(c => categoriesArr.Contains(c.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

                foreach (var book in books)
                {
                    sb.AppendLine(book);
                }
            }

            return sb.ToString().Trim();
        }

        //Problem 7
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new();

            var books = context.Books
                .AsNoTracking()
                .Where(b => b.ReleaseDate.Value < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                })
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return sb.ToString().Trim();
        }

        //Problem 8
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new();

            var authors = context.Authors
                .AsNoTracking()
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName
                })
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .ToArray();

            foreach (var auth in authors)
            {
                sb.AppendLine($"{auth.FirstName} {auth.LastName}");
            }

            return sb.ToString().Trim();
        }

        //Problem 9
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            StringBuilder sb = new();

            IEnumerable<string> books = context.Books
                .AsNoTracking()
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            foreach (var item in books)
            {
                sb.AppendLine(item);
            }

            return sb.ToString().Trim();
        }

        //Problem 10
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new();

            var books = context.Books
                .AsNoTracking()
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    b.Author.FirstName,
                    b.Author.LastName
                })
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.FirstName} {book.LastName})");
            }

            return sb.ToString().Trim();
        }

        //Problem 11
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int result = 0;

            return result = context.Books
                .AsNoTracking()
                .Where(b => b.Title.Length > lengthCheck)
                .Count();
        }

        //Problem 12
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new();

            var copies = context.Authors
                .AsNoTracking()
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    TotalCopies = a.Books
                    .Sum(b => b.Copies),
                })
                .OrderByDescending(a => a.TotalCopies)
                .ToArray();

            foreach (var author in copies)
            {
                sb.AppendLine($"{author.FirstName} {author.LastName} - {author.TotalCopies}");
            }

            return sb.ToString().Trim();
        }

        //Problem 13
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new();

            var profit = context.Categories
                .AsNoTracking()
                .Select(c => new
                {
                    c.Name,
                    Profit = c.CategoryBooks
                        .Sum(bc => bc.Book.Copies * bc.Book.Price)
                })
                .OrderByDescending(c => c.Profit)
                .ThenBy(c => c.Name)
                .ToArray();

            foreach (var category in profit)
            {
                sb.AppendLine($"{category.Name} ${category.Profit:f2}");
            }

            return sb.ToString().Trim();
        }

        //Problem 14
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new();

            var books = context.Categories
                .AsNoTracking()
                .Select(c => new
                {
                    c.Name,
                    RecentBooks = c.CategoryBooks
                        .Select(bc => new
                        {
                            Booktitle = bc.Book.Title,
                            bc.Book.ReleaseDate.Value.Year
                        })
                        .OrderByDescending(b => b.Year)
                        .Take(3)
                        .ToArray()
                })
                .OrderBy(c => c.Name)
                .ToArray();

            foreach (var category in books)
            {
                sb.AppendLine($"--{category.Name}");
                foreach (var book in category.RecentBooks)
                {
                    sb.AppendLine($"{book.Booktitle} ({book.Year})");
                }
            }

            return sb.ToString().Trim();
        }

        //Problem 15
        public static void IncreasePrices(BookShopContext context)
        {
            foreach (var book in context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010))
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        //Problem 16
        public static int RemoveBooks(BookShopContext context)
        {
            var booksToRemove = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            int count = booksToRemove.Count;

            context.Books.RemoveRange(booksToRemove);

            context.SaveChanges();

            return count;
        }
    }
}


