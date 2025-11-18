namespace Cadastre.Data
{
    using Cadastre.Data.Models;
    using Microsoft.EntityFrameworkCore;

    using static DataValidation.DataValidation.District;
    using static DataValidation.DataValidation.Property;
    using static DataValidation.DataValidation.Citizen;

    public class CadastreContext : DbContext
    {
        public CadastreContext()
        {
            
        }

        public CadastreContext(DbContextOptions options)
            :base(options)
        {
            
        }

        public virtual DbSet<District> Districts { get; set; } = null!;
        public virtual DbSet<Property> Properties { get; set; } = null!;
        public virtual DbSet<Citizen> Citizens { get; set; } = null!;
        public virtual DbSet<PropertyCitizen> PropertiesCitizens { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<District>()
                .HasCheckConstraint("CK_Name_Between",
                 $"LEN([Name]) >= {MinNameLength} AND LEN([Name]) <= {MaxNameLength}");

            modelBuilder.Entity<District>()
                .Property(d => d.PostalCode)
                .HasColumnType("NCHAR(8)");

            modelBuilder.Entity<District>()
                .HasCheckConstraint("CK_PostalCode_Value",
                "[PostalCode] LIKE '[A-Z][A-Z]-[0-9][0-9][0-9][0-9][0-9]'");

            modelBuilder.Entity<Property>()
                .HasCheckConstraint("CK_PropertyIdentifier_Length",
                $"LEN([PropertyIdentifier]) >= {MinPropertyIdentifierLength} AND LEN([PropertyIdentifier]) <= {MaxPropertyIdentifierLength}");

            modelBuilder.Entity<Property>()
                .HasCheckConstraint("CK_Area_Must_Be_Positive",
                "[Area] >= 0");

            modelBuilder.Entity<Property>()
                .HasCheckConstraint("CK_Details_Length",
                $"LEN([Details]) >= {MinDetailsLength} AND LEN([Details]) <= {MaxDetailsLength}");

            modelBuilder.Entity<Property>()
                .HasCheckConstraint("CK_Address_Length",
                $"LEN([Address]) >= {MinAddressLength} AND LEN([Address]) <= {MaxAddressLength}");

            modelBuilder.Entity<Property>()
                .HasOne(p => p.District)
                .WithMany(d => d.Properties)
                .HasForeignKey(p => p.DistrictId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Citizen>()
                .HasCheckConstraint("CK_FirstName_Length",
                $"LEN([FirstName]) >= {MinFirstNameLength} AND LEN([FirstName]) <= {MaxFirstNameLength}");

            modelBuilder.Entity<Citizen>()
                .HasCheckConstraint("CK_LastName_Length",
                $"LEN([LastName]) >= {MinLastNameLength} AND LEN([LastName]) <= {MaxLastNameLength}");

            modelBuilder.Entity<PropertyCitizen>()
                .HasKey(pc => new { pc.PropertyId, pc.CitizenId });

            modelBuilder.Entity<PropertyCitizen>()
                .HasOne(pc => pc.Property)
                .WithMany(p => p.PropertiesCitizens)
                .HasForeignKey(pc => pc.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PropertyCitizen>()
                .HasOne(pc => pc.Citizen)
                .WithMany(c => c.PropertiesCitizens)
                .HasForeignKey(pc => pc.CitizenId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
