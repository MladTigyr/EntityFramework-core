namespace Boardgames.Data
{
    using Boardgames.Data.Models;
    using Microsoft.EntityFrameworkCore;

    using static Common.EntityValidation;
    
    public class BoardgamesContext : DbContext
    {
        public BoardgamesContext()
        { 

        }

        public BoardgamesContext(DbContextOptions options)
            : base(options) 
        {

        }

        public virtual DbSet<Creator> Creators { get; set; } = null!;

        public virtual DbSet<Boardgame> Boardgames { get; set; } = null!;

        public virtual DbSet<Seller> Sellers { get; set; } = null!;

        public virtual DbSet<BoardgameSeller> BoardgamesSellers { get; set; } = null!;

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
            modelBuilder.Entity<Boardgame>()
                .HasCheckConstraint("CK_Boardgame_NameLength",
                $"LEN([Name]) >= {MinBoardGameNameLength} AND LEN([Name]) <= {MaxBoardGameNameLength}");

            modelBuilder.Entity<Boardgame>()
                .HasCheckConstraint("CK_Boardgame_Rating",
                $"[Rating] >= {MinBoardGameRatingValue} AND [Rating] <= {MaxBoardGameRatingValue}");

            modelBuilder.Entity<Boardgame>()
                .HasCheckConstraint("CK_Boardgame_YearPublished",
                $"[YearPublished] >= {MinBoardGameYearPublished} AND [YearPublished] <= {MaxBoardGameYearPublished}");

            modelBuilder.Entity<Boardgame>(entity =>
            {
                entity.HasOne(e => e.Creator)
                .WithMany(c => c.Boardgames)
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Creator>()
                .HasCheckConstraint("CK_Creator_FirstNameLength",
                $"LEN([FirstName]) >= {MinCreatorFirstNameLength} AND LEN([FirstName]) <= {MaxCreatorFirstNameLength}");

            modelBuilder.Entity<Creator>()
                .HasCheckConstraint("CK_Creator_LastNameLength",
                $"LEN([LastName]) >= {MinCreatorLastNameLength} AND LEN([LastName]) <= {MaxCreatorLastNameLength}");

            modelBuilder.Entity<Seller>()
                .HasCheckConstraint("CK_Seller_NameLength",
                $"LEN([Name]) >= {MinSellerNameLength} AND LEN([Name]) <= {MaxSellerNameLength}");

            modelBuilder.Entity<Seller>()
                .HasCheckConstraint("CK_Seller_AddressLength",
                $"LEN([Address]) >= {MinSellerAddressLength} AND LEN([Address]) <= {MaxSellerAddressLength}");

            modelBuilder.Entity<BoardgameSeller>()
                .HasKey(bs => new { bs.BoardgameId, bs.SellerId });

            modelBuilder.Entity<BoardgameSeller>(entity =>
            {
                entity.HasOne(bs => bs.Boardgame)
                .WithMany(b => b.BoardgamesSellers)
                .HasForeignKey(bs => bs.BoardgameId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(bs => bs.Seller)
                .WithMany(s => s.BoardgamesSellers)
                .HasForeignKey(bs => bs.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}
