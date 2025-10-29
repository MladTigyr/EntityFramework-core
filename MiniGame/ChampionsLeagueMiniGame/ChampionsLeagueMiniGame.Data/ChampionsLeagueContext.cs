using ChampionsLeagueMiniGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChampionsLeagueMiniGame.Data
{
    public class ChampionsLeagueContext : DbContext
    {
        public ChampionsLeagueContext() 
        {

        }

        public ChampionsLeagueContext(DbContextOptions<ChampionsLeagueContext> options) 
            : base(options) 
        {

        }

        public virtual DbSet<Team> Teams { get; set; }

        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Nationality>  Nationalities { get; set; }

        public virtual DbSet<Match> Matches { get; set; }

        public virtual DbSet<Position> Positions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-B752TI8\\SQLEXPRESS;Database=ChampionsLeague;Trusted_Connection=True;Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasOne(t => t.Nationality)
                    .WithMany(n => n.Teams)
                    .HasForeignKey(t => t.NationalityId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasIndex(p => new { p.PlayerId, p.ShirtNumber })
                    .IsUnique();

                entity.HasOne(p => p.Postion)
                    .WithMany(po => po.Players)
                    .HasForeignKey(p => p.PostitionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Team)
                    .WithMany(t => t.Players)
                    .HasForeignKey(p => p.TeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Nationality)
                    .WithMany(n => n.Players)
                    .HasForeignKey(p => p.NationalityId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasOne(m => m.HomeTeam)
                    .WithMany(t => t.HomeMatches)
                    .HasForeignKey(m => m.HomeTeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.AwayTeam)
                    .WithMany(t => t.AwayMatches)
                    .HasForeignKey(m => m.AwayTeamId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }

        public void Reset()
        {

        }
    }
}
