using ChampionsLeagueMiniGame.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Bogus;
using ChampionsLeagueMiniGame.Data.Models;
using ChampionsLeagueMiniGame.Data.Models.Enums;

namespace ChampionsLeagueMiniGame.Core.Services
{
    public class DbInitializer
    {
        public static void ResetDatabase(ChampionsLeagueContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Console.WriteLine("The Database was created successfully");

            Seed(context);
        }

        private static void Seed(ChampionsLeagueContext context)
        {
            Random rnd = new Random();

            Nationality n1 = new Nationality { Name = "England" };
            Nationality n2 = new Nationality { Name = "France" };
            Nationality n3 = new Nationality { Name = "Germany" };
            Nationality n4 = new Nationality { Name = "Spain" };
            Nationality n5 = new Nationality { Name = "Italy" };
            Nationality n6 = new Nationality { Name = "Bulgaria" };
            Nationality n7 = new Nationality { Name = "Belgium" };
            Nationality n8 = new Nationality { Name = "Portugal" };
            Nationality n9 = new Nationality { Name = "Turkey" };
            Nationality n10 = new Nationality { Name = "Netherlands" };

            List<Nationality> nationalities = new List<Nationality>() { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10};

            context.AddRange(nationalities);

            Position forward = new Position { PositionType = (Data.Models.Enums.PositionType)0 };
            Position midfielder = new Position { PositionType = (Data.Models.Enums.PositionType)1 };
            Position defender = new Position { PositionType = (Data.Models.Enums.PositionType)2 };
            Position goalkeeper = new Position { PositionType = (Data.Models.Enums.PositionType)3 };

            List<Position> positions = new List<Position>() { forward, midfielder, defender, goalkeeper };

            context.AddRange(positions);

            Team arsenal = new Team { Name = "Arsenal", Nationality = n1 };
            Team psg = new Team { Name = "Paris Saint-Germain", Nationality = n2 };
            Team monaco = new Team { Name = "Monaco", Nationality = n2 };
            Team lyon = new Team { Name = "Lyon", Nationality = n2 };
            Team marseille = new Team { Name = "Marseille", Nationality = n2 };
            Team bayern = new Team { Name = "Bayern Munich", Nationality = n3 };
            Team leverkusen = new Team { Name = "Bayer Leverkusen", Nationality = n3 };
            Team frankfurt = new Team { Name = "Eintracht Frankfurt", Nationality = n3 };
            Team dortmund = new Team { Name = "Borussia Dortmund", Nationality = n3 };
            Team realMadrid = new Team { Name = "Real Madrid", Nationality = n4 };
            Team barcelona = new Team { Name = "Barcelona", Nationality = n4 };
            Team atletico = new Team { Name = "Atlético Madrid", Nationality = n4 };
            Team athletic = new Team { Name = "Athletic Club", Nationality = n4 };
            Team villarreal = new Team { Name = "Villarreal", Nationality = n4 };
            Team napoli = new Team { Name = "Napoli", Nationality = n5 };
            Team inter = new Team { Name = "Inter Milan", Nationality = n5 };
            Team atalanta = new Team { Name = "Atalanta", Nationality = n5 };
            Team juventus = new Team { Name = "Juventus", Nationality = n5 };
            Team levski = new Team { Name = "Levski Sofia", Nationality = n6 };
            Team cska = new Team { Name = "CSKA Sofia", Nationality = n6 };
            Team union = new Team { Name = "Union Saint-Gilloise", Nationality = n7 };
            Team anderlecht = new Team { Name = "RSC Anderlecht", Nationality = n7 };
            Team sporting = new Team { Name = "Sporting CP", Nationality = n8 };
            Team porto = new Team { Name = "FC Porto", Nationality = n8 };
            Team benfica = new Team { Name = "Benfica", Nationality = n8 };
            Team galatasaray = new Team { Name = "Galatasaray", Nationality = n9 };
            Team besiktas = new Team { Name = "Besiktas", Nationality = n9 };
            Team ajax = new Team { Name = "Ajax", Nationality = n10 };
            Team psv = new Team { Name = "PSV Eindhoven", Nationality = n10 };
            Team feyenoord = new Team { Name = "Feyenoord", Nationality = n10 };
            Team sevilla = new Team { Name = "Sevilla", Nationality = n4 };
            Team roma = new Team { Name = "AS Roma", Nationality = n5 };
            Team tottenham = new Team { Name = "Tottenham Hotspur", Nationality = n1 };
            Team liverpool = new Team { Name = "Liverpool", Nationality = n1 };
            Team manCity = new Team { Name = "Manchester City", Nationality = n1 };
            Team chelsea = new Team { Name = "Chelsea", Nationality = n1 };

            context.Teams.AddRange(
                arsenal, psg, monaco, lyon, marseille,
                bayern, leverkusen, frankfurt, dortmund,
                realMadrid, barcelona, atletico, athletic, villarreal, sevilla,
                napoli, inter, atalanta, juventus, roma,
                levski, cska,
                union, anderlecht,
                sporting, porto, benfica,
                galatasaray, besiktas,
                ajax, psv, feyenoord,
                tottenham, liverpool, manCity, chelsea
            );

            foreach(var team in context.Teams)
            {
                var currTeamgoalkeepers = GeneratePlayersForTeam(team, goalkeeper, 2, nationalities);
                var currTeamDefenders = GeneratePlayersForTeam(team, defender, 8, nationalities);
                var currTeamMidfielders = GeneratePlayersForTeam(team, midfielder, 10, nationalities);
                var currTeamForwards = GeneratePlayersForTeam(team, forward, 6, nationalities);

                var currPlayers = currTeamgoalkeepers
                    .Concat(currTeamDefenders)
                    .Concat(currTeamMidfielders)
                    .Concat(currTeamForwards)
                    .ToList();

                context.AddRange(currPlayers);
            }
        }

        private static List<Player> GeneratePlayersForTeam(
            Team team,
            Position position,
            int count,
            List<Nationality> nationalities)
        {
            var faker = new Faker<Player>()
                .RuleFor(p => p.FirstName, f => f.Person.FirstName)
                .RuleFor(p => p.LastName, f => f.Person.LastName)
                .RuleFor(p => p.Age, f => f.Random.Int(18, 36))
                .RuleFor(p => p.Rating, f => f.Random.Int(70, 99)) 
                .RuleFor(p => p.PrefferedFoot, f => f.PickRandom<PrefferedFoot>())
                .RuleFor(p => p.ShirtNumber, f => f.Random.Int(1, 99))
                .RuleFor(p => p.Postion, f => position)
                .RuleFor(p => p.Nationality, f => f.PickRandom(nationalities))
                .RuleFor(p => p.Team, f => team);

            return faker.Generate(count);
        }
    }
}
