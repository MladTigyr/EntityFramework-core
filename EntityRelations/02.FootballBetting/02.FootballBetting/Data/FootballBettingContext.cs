using Microsoft.EntityFrameworkCore;
using P02_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {
        }
        public FootballBettingContext(DbContextOptions options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-B752TI8\\SQLEXPRESS;Database=FootballBetting;Integrated Security=True;");
            }
        }

        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Town> Towns { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<PlayerStatistic> PlayersStatistics { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Bet> Bets { get; set; }
        public virtual DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(t => t.Name)
                    .HasMaxLength(100)
                    .IsUnicode(true)
                    .IsRequired();

                entity.Property(t => t.Initials)
                      .IsFixedLength()
                      .HasMaxLength(3)
                      .IsRequired()
                      .IsUnicode(false);

                entity.Property(t => t.Budget)
                    .HasColumnType("money")
                    .IsRequired();

                entity.HasOne(t => t.PrimaryKitColor)
                      .WithMany(c => c.PrimaryKitTeams)
                      .HasForeignKey(t => t.PrimaryKitColorId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(t => t.SecondaryKitColor)
                      .WithMany(c => c.SecondaryKitTeams)
                      .HasForeignKey(t => t.SecondaryKitColorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Town)
                    .WithMany(t => t.Teams)
                    .HasForeignKey(t => t.TownId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.Property(c => c.Name)
                    .HasMaxLength(30)
                    .IsUnicode(true)
                    .IsRequired();
            });

            modelBuilder.Entity<Town>(entity =>
            {
                entity.Property(t => t.Name)
                    .HasMaxLength(80)
                    .IsUnicode(true)
                    .IsRequired();

                entity.HasOne(t => t.Country)
                    .WithMany(c => c.Towns)
                    .HasForeignKey(t => t.CountryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(c => c.Name)
                    .HasMaxLength(80)
                    .IsUnicode(true)
                    .IsRequired();
            });


            modelBuilder.Entity<Player>()
                .HasCheckConstraint("CK_Player_SquadNumber", "[SquadNumber] BETWEEN 1 AND 99");

            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(p => p.Name)
                    .HasMaxLength(100)
                    .IsUnicode(true)
                    .IsRequired();

                entity.Property(p => p.SquadNumber)
                    .IsRequired();

                entity.HasIndex(p => new { p.TeamId, p.SquadNumber })
                    .IsUnique();

                entity.HasOne(p => p.Position)
                    .WithMany(p => p.Players)
                    .HasForeignKey(p => p.PositionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Team)
                    .WithMany(t => t.Players)
                    .HasForeignKey(p => p.TeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Town)
                    .WithMany(t => t.Players)
                    .HasForeignKey(p => p.TownId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(p => p.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();
            });

            modelBuilder.Entity<PlayerStatistic>()
                .HasKey(ps => new { ps.PlayerId, ps.GameId });

            modelBuilder.Entity<PlayerStatistic>(entity =>
            {
                entity.Property(ps => ps.ScoredGoals)
                    .IsRequired();

                entity.Property(ps => ps.Assists)
                    .IsRequired();

                entity.Property(ps => ps.MinutesPlayed)
                    .IsRequired();

                entity.HasOne(ps => ps.Player)
                    .WithMany(p => p.PlayersStatistics)
                    .HasForeignKey(ps => ps.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ps => ps.Game)
                    .WithMany(g => g.PlayersStatistics)
                    .HasForeignKey(ps => ps.GameId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(g => g.GameId)
                    .IsRequired();

                entity.Property(g => g.HomeTeamGoals)
                    .IsRequired();

                entity.Property(g => g.AwayTeamGoals)
                    .IsRequired();

                entity.Property(g => g.HomeTeamBetRate)
                    .IsRequired();

                entity.Property(g => g.AwayTeamBetRate)
                    .IsRequired();

                entity.Property(g => g.DrawBetRate)
                    .IsRequired();

                entity.Property(g => g.DateTime)
                    .IsRequired();

                entity.Property(g => g.Result)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsRequired();

                entity.HasOne(g => g.HomeTeam)
                    .WithMany(t => t.HomeGames)
                    .HasForeignKey(g => g.HomeTeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(g => g.AwayTeam)
                    .WithMany(t => t.AwayGames)
                    .HasForeignKey(g => g.AwayTeamId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Bet>(entity =>
            {
                entity.Property(b => b.BetId)
                    .IsRequired();

                entity.Property(b => b.Amount)
                    .HasColumnType("money")
                    .IsRequired();

                entity.Property(b => b.Prediction)
                    .IsRequired();

                entity.Property(b => b.DateTime)
                    .IsRequired();

                entity.HasOne(b => b.User)
                    .WithMany(u => u.Bets)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Game)
                    .WithMany(g => g.Bets)
                    .HasForeignKey(b => b.GameId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(u => u.Name)
                    .HasMaxLength(100)
                    .IsUnicode(true)
                    .IsRequired();

                entity.Property(u => u.Password)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(u => u.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(u => u.Balance)
                    .HasColumnType("money")
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
