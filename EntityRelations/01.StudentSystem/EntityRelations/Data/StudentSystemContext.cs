using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
        }
        
        public StudentSystemContext(DbContextOptions<StudentSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Student> Students { get; set; } = null!;

        public virtual DbSet<Course> Courses { get; set; } = null!;

        public virtual DbSet<Resource> Resources { get; set; } = null!;

        public virtual DbSet<Homework> Homeworks { get; set; } = null!;

        public virtual DbSet<StudentCourse> StudentsCourses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-B752TI8\\SQLEXPRESS;Database=StudentSystem;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsUnicode()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .IsUnicode(false)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(c => c.Name)
                    .IsUnicode()
                    .HasMaxLength(80);

                entity.Property(c => c.Description)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.Property(r => r.Name)
                    .IsUnicode()
                    .HasMaxLength(50);
                
                entity.Property(r => r.Url)
                .IsUnicode(false);

                entity.HasOne(r => r.Course)
                    .WithMany(c => c.Resources)
                    .HasForeignKey(r => r.CourseId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Resources_Courses");
            });

            modelBuilder.Entity<Homework>(entity =>
            {
                entity.Property(h => h.Content)
                    .IsUnicode(false);

                entity.HasOne(h => h.Student)
                    .WithMany(s => s.Homeworks)
                    .HasForeignKey(h => h.StudentId)
                    .HasConstraintName("FK_Homeworks_Students")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(h => h.Course)
                    .WithMany(c => c.Homeworks)
                    .HasForeignKey(h => h.CourseId)
                    .HasConstraintName("FK_Homeworks_Courses")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(sc => new { sc.StudentId, sc.CourseId });

                entity.HasOne(sc => sc.Student)
                    .WithMany(s => s.StudentsCourses)
                    .HasForeignKey(sc => sc.StudentId)
                    .HasConstraintName("FK_StudentsCourses_Students")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sc => sc.Course)
                    .WithMany(c => c.StudentsCourses)
                    .HasForeignKey(sc => sc.CourseId)
                    .HasConstraintName("FK_StudentsCourses_Courses")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
