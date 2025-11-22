namespace Medicines.Data
{
    using Medicines.Data.Models;
    using Microsoft.EntityFrameworkCore;

    using static EntityValidation;

    public class MedicinesContext : DbContext
    {
        public MedicinesContext()
        {

        }

        public MedicinesContext(DbContextOptions options)
            : base(options)
        {

        }

        public virtual DbSet<Pharmacy> Pharmacies { get; set; } = null!;

        public virtual DbSet<Medicine> Medicines { get; set; } = null!;

        public virtual DbSet<Patient> Patients { get; set; } = null!;

        public virtual DbSet<PatientMedicine> PatientsMedicines { get; set; } = null!;

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
            modelBuilder.Entity<Pharmacy>()
                .HasCheckConstraint("CK_Pharmacy_NameLength",
                $"LEN([Name]) >= {MinPharmacyNameLength} AND LEN([Name]) <= {MaxPharmacyNameLength}");

            modelBuilder.Entity<Pharmacy>()
                .Property(p => p.PhoneNumber)
                .HasColumnType("NCHAR(14)");

            modelBuilder.Entity<Medicine>()
                .HasCheckConstraint("CK_Medicine_NameLength",
                $"LEN([Name]) >= {MinMedicineNameLength} AND LEN([Name]) <= {MaxMedicineNameLength}");

            modelBuilder.Entity<Medicine>()
                .HasCheckConstraint("CK_Medicine_PriceCheck",
                $"[Price] >= {MinMedicinePrice} AND [Price] <= {MaxMedicinePrice}");

            modelBuilder.Entity<Medicine>()
                .HasCheckConstraint("CK_Medicine_ProducerLength",
                $"LEN([Producer]) >= {MinProducerMedicineLength} AND LEN([Producer]) <= {MaxProducerMedicineLength}");

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.HasOne(m => m.Pharmacy)
                .WithMany(p => p.Medicines)
                .HasForeignKey(m => m.PharmacyId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Patient>()
                .HasCheckConstraint("CK_Patient_FullNameLength",
                $"LEN([FullName]) >= {MinPatientFullNameLength} AND LEN([FullName]) <= {MaxPatientFullNameLength}");

            modelBuilder.Entity<PatientMedicine>()
                .HasKey(pm => new { pm.PatientId, pm.MedicineId });

            modelBuilder.Entity<PatientMedicine>(entity =>
            {
                entity.HasOne(pm => pm.Medicine)
                .WithMany(m => m.PatientsMedicines)
                .HasForeignKey(pm => pm.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(pm => pm.Patient)
                .WithMany(p => p.PatientsMedicines)
                .HasForeignKey(pm => pm.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
