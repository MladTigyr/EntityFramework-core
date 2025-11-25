namespace Trucks.Data
{
    using Microsoft.EntityFrameworkCore;
    using Trucks.Data.Models;

    using static Common.EntityValidation;

    public class TrucksContext : DbContext
    {
        public TrucksContext()
        { 

        }

        public TrucksContext(DbContextOptions options)
            : base(options) 
        { 

        }

        public virtual DbSet<Despatcher> Despatchers { get; set; } = null!;
        public virtual DbSet<Truck> Trucks { get; set; } = null!;
        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<ClientTruck>  ClientsTrucks { get; set; } = null!;

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
            modelBuilder.Entity<Truck>()
                .Property(t => t.VinNumber)
                .HasColumnType("NCHAR(17)");

            modelBuilder.Entity<Truck>()
                .HasCheckConstraint("CK_Truck_TankCapacityValue",
                $"[TankCapacity] >= {MinTruckTankCapacityValue} AND [TankCapacity] <= {MaxTruckTankCapacityValue}");

            modelBuilder.Entity<Truck>()
                .HasCheckConstraint("CK_Truck_CargoCapacityValue",
                $"[CargoCapacity] >= {MinTruckCargoCapacityValue} AND [CargoCapacity] <= {MaxTruckCargoCapacityValue}");

            modelBuilder.Entity<Truck>(entity =>
            {
                entity.HasOne(t => t.Despatcher)
                .WithMany(d => d.Trucks)
                .HasForeignKey(t => t.DespatcherId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Client>()
                .HasCheckConstraint("CK_Client_NameLength",
                $"LEN([Name]) >= {MinClientNameLength} AND LEN([Name]) <= {MaxClientNameLength}");

            modelBuilder.Entity<Client>()
                .HasCheckConstraint("CK_Client_NationalityLength",
                $"LEN([Nationality]) >= {MinClientNationalityLength} AND LEN([Nationality]) <= {MaxClientNationalityLength}");

            modelBuilder.Entity<Despatcher>()
                .HasCheckConstraint("CK_Despatcher_NameLength",
                $"LEN([Name]) >= {MinDespatcherNameLength} AND LEN([Name]) <= {MaxDespatcherNameLength}");

            modelBuilder.Entity<ClientTruck>()
                .HasKey(ct => new { ct.ClientId, ct.TruckId });

            modelBuilder.Entity<ClientTruck>(entity =>
            {
                entity.HasOne(ct => ct.Client)
                .WithMany(c => c.ClientsTrucks)
                .HasForeignKey(ct => ct.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ct => ct.Truck)
                .WithMany(t => t.ClientsTrucks)
                .HasForeignKey(ct => ct.TruckId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
