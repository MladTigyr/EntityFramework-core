using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data

{
    public class HospitalContext : DbContext
    {
        public HospitalContext() { }

        public HospitalContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Patient> Patients { get; set; }

        public virtual DbSet<Visitation> Visitations { get; set; }

        public virtual DbSet<Diagnose> Diagnoses { get; set; }

        public virtual DbSet<Medicament> Medicaments { get; set; }

        public virtual DbSet<PatientMedicament> PatientsMedicaments { get; set; }

        public virtual DbSet<Doctor> Doctors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-B752TI8\\SQLEXPRESS;Database=Hospital;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Visitation>(entity =>
            {
                entity.HasOne(e => e.Patient)
                    .WithMany(p => p.Visitations)
                    .HasForeignKey(e => e.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Doctor)
                    .WithMany(d => d.Visitations)
                    .HasForeignKey(e => e.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Diagnose>(entity =>
            {
                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Diagnoses)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity.HasOne(pm => pm.Patient)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(pm =>  pm.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(pm => pm.Medicament)
                    .WithMany(m => m.Prescriptions)
                    .HasForeignKey(pm => pm.MedicamentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
