namespace Artillery.Data
{
    using static Common.EntityValidation;
    using Artillery.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public class ArtilleryContext : DbContext
    {
        public ArtilleryContext() 
        { 

        }

        public ArtilleryContext(DbContextOptions options)
            : base(options) 
        { 

        }

        public virtual DbSet<Country> Countries { get; set; } = null!;

        public virtual DbSet<Gun> Guns { get; set; } = null!;

        public virtual DbSet<Manufacturer> Manufacturers { get; set; } = null!;

        public virtual DbSet<Shell> Shells { get; set; } = null!;

        public virtual DbSet<CountryGun> CountriesGuns { get; set; } = null!;

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
            modelBuilder.Entity<Country>()
                .HasCheckConstraint("CK_Country_NameLength",
                $"LEN([CountryName]) >= {MinCountryNameLength} AND LEN([CountryName]) <= {MaxCountryNameLength}");

            modelBuilder.Entity<Country>()
                .HasCheckConstraint("CK_Country_ArmySize",
                $"[ArmySize] >= {MinCountryArmySize} AND [ArmySize] <= {MaxCountryArmySize}");

            modelBuilder.Entity<Manufacturer>()
                .HasCheckConstraint("CK_Manufacturer_NameLength",
                $"LEN([ManufacturerName]) >= {MinManufacturerNameLength} AND LEN([ManufacturerName]) <= {MaxManufacturerNameLength}")
                .HasIndex(m => m.ManufacturerName)
                .IsUnique();

            modelBuilder.Entity<Manufacturer>()
                .HasCheckConstraint("CK_Manufacturer_FoundedLength",
                $"LEN([Founded]) >= {MinManufacturerFounded} AND LEN([Founded]) <= {MaxManufacturerFounded}");

            modelBuilder.Entity<Shell>()
                .HasCheckConstraint("CK_Shell_ShellWeight",
                $"[ShellWeight] >= {MinShellWeight} AND [ShellWeight] <= {MaxShellWeight}");

            modelBuilder.Entity<Shell>()
                .HasCheckConstraint("CK_Shell_CaliberLength",
                $"LEN([Caliber]) >= {MinShellCaliberLength} AND LEN([Caliber]) <= {MaxShellCaliberLength}");

            modelBuilder.Entity<Gun>()
                .HasCheckConstraint("CK_Gun_Weight",
                $"[GunWeight] >= {MinGunWeight} AND [GunWeight] <= {MaxGunWeight}");

            modelBuilder.Entity<Gun>()
                .HasCheckConstraint("CK_Gun_BarrelLength",
                $"[BarrelLength] >= {MinGunBarrelLength} AND [BarrelLength] <= {MaxGunBarrelLength}");

            modelBuilder.Entity<Gun>()
                .HasCheckConstraint("CK_Gun_Range",
                $"[Range] >= {MinGunRange} AND [Range] <= {MaxGunRange}");

            modelBuilder.Entity<Gun>(entity =>
            {
                entity.HasOne(g => g.Manufacturer)
                .WithMany(m => m.Guns)
                .HasForeignKey(g => g.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(g => g.Shell)
                .WithMany(s => s.Guns)
                .HasForeignKey(g => g.ShellId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CountryGun>()
                .HasKey(cg => new { cg.CountryId, cg.GunId });

            modelBuilder.Entity<CountryGun>(entity =>
            {
                entity.HasOne(cg => cg.Country)
                .WithMany(c => c.CountriesGuns)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(cg => cg.Gun)
                .WithMany(g => g.CountriesGuns)
                .HasForeignKey (g => g.GunId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
