namespace Footballers.Data
{
    using static Common.EntityValidation;   
    using Footballers.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class FootballersContext : DbContext
    {
        public FootballersContext() 
        {

        }

        public FootballersContext(DbContextOptions options)
            : base(options) 
        {

        }

        public virtual DbSet<Coach> Coaches { get; set; } = null!;

        public virtual DbSet<Footballer> Footballers { get; set; } = null!;

        public virtual DbSet<Team> Teams { get; set; } = null!;

        public virtual DbSet<TeamFootballer> TeamsFootballers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Footballer>()
                .HasCheckConstraint("CK_Footballer_NameLength",
                $"LEN([Name]) >= {MinFootballerNameLength} AND LEN([Name]) <= {MaxFootballerNameLength}");

            modelBuilder.Entity<Footballer>(entity =>
            {
                entity.HasOne(f => f.Coach)
                .WithMany(c => c.Footballers)
                .HasForeignKey(f => f.CoachId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Team>()
                .HasCheckConstraint("CK_Team_NameLength",
                $"LEN([Name]) >= {MinTeamNameLength} AND LEN([Name]) <= {MaxTeamNameLength}");

            modelBuilder.Entity<Team>()
                .HasCheckConstraint("CK_Team_NationalityLength",
                $"LEN([Nationality]) >= {MinTeamNationalityLength} AND LEN([Nationality]) <= {MaxTeamNationalityLength}");

            modelBuilder.Entity<Coach>()
                .HasCheckConstraint("CK_Coach_NameLength",
                $"LEN([Name]) >= {MinCoachNameLength} AND LEN([Name]) <= {MaxCoachNameLength}");

            modelBuilder.Entity<TeamFootballer>()
                .HasKey(tf => new { tf.TeamId, tf.FootballerId });

            modelBuilder.Entity<TeamFootballer>(entity =>
            {
                entity.HasOne(tf => tf.Team)
                .WithMany(t => t.TeamsFootballers)
                .HasForeignKey(tf => tf.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tf => tf.Footballer)
                .WithMany(f => f.TeamsFootballers)
                .HasForeignKey(tf => tf.FootballerId);
            });
        }
    }
}
