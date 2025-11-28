
namespace Theatre.Data
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using static Common.EntityValidation;


    public class TheatreContext : DbContext
    {
        public TheatreContext() 
        {

        }

        public TheatreContext(DbContextOptions options)
        : base(options) 
        { 

        }

        public virtual DbSet<Theatre> Theatres { get; set; } = null!;

        public virtual DbSet<Cast> Casts { get; set; } = null!;

        public virtual DbSet<Ticket> Tickets { get; set; } = null!;

        public virtual DbSet<Play> Plays { get; set; } = null!;

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
            modelBuilder.Entity<Theatre>()
                .HasCheckConstraint("CK_Theatre_NameLength",
                $"LEN([Name]) >= {MinTheatreNameLength} AND LEN([Name]) <= {MaxTheatreNameLength}");

            modelBuilder.Entity<Theatre>()
                .HasCheckConstraint("CK_Theatre_NumberOfHalls",
                $"[NumberOfHalls] >= {MinTheatreNumberOfHalls} AND [NumberOfHalls] <= {MaxTheatreNumberOfHalls}");

            modelBuilder.Entity<Theatre>()
                .HasCheckConstraint("CK_Theatre_DirectorLength",
                $"LEN([Director]) >= {MinTheatreDirectorLength} AND LEN([Director]) <= {MaxTheatreDirectorLength}");

            modelBuilder.Entity<Play>()
                .HasCheckConstraint("CK_Play_TitleLength",
                $"LEN([Title]) >= {MinPlayTitleLength} AND LEN([Title]) <= {MaxPlayTitleLength}");

            modelBuilder.Entity<Play>()
                .HasCheckConstraint("CK_Play_Duration",
                $"[Duration] >= '{PlayDuration}'");

            modelBuilder.Entity<Play>()
                .HasCheckConstraint("CK_Play_Rating",
                $"[Rating] >= {MinPlayRatinValue} AND [Rating] <= {MaxPlayRatinValue}");

            modelBuilder.Entity<Play>()
                .HasCheckConstraint("CK_Play_DescriptionLength",
                $"LEN([Description]) >= {MinPlayDescriptionLength} AND LEN([Description]) <= {MaxPlayDescriptionLength}");

            modelBuilder.Entity<Play>()
               .HasCheckConstraint("CK_Play_ScreenwriterLength",
               $"LEN([Screenwriter]) >= {MinPlayScreeWriterLength} AND LEN([Screenwriter]) <= {MaxPlayScreeWriterLength}");

            modelBuilder.Entity<Cast>()
                .HasCheckConstraint("CK_Cast_FullNameLength",
                $"LEN([FullName]) >= {MinCastNameLength} AND LEN([FullName]) <= {MaxCastNameLength}");

            modelBuilder.Entity<Cast>()
                .Property(c => c.PhoneNumber)
                .HasColumnType("NCHAR(15)");

            modelBuilder.Entity<Cast>(entity =>
            {
                entity.HasOne(c => c.Play)
                .WithMany(p => p.Casts)
                .HasForeignKey(c => c.PlayId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Ticket>()
                .HasCheckConstraint("CK_Ticket_Price",
                $"[Price] >= {MinTicketPriceValue} AND [Price] <= {MaxTicketPriceValue}");

            modelBuilder.Entity<Ticket>()
                .HasCheckConstraint("CK_Ticket_RowNumber",
                $"[RowNumber] >= {MinTickerRowNum} AND [RowNumber] <= {MaxTickerRowNum}");

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasOne(t => t.Play)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.PlayId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Theatre)
                .WithMany(th => th.Tickets)
                .HasForeignKey(t => t.TheatreId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}