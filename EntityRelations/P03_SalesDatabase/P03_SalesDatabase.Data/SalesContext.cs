using Bogus;
using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public SalesContext() { }

        public SalesContext(DbContextOptions options) 
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Store> Stores { get; set; }

        public virtual DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-B752TI8\\SQLEXPRESS;Database=Sales;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(c => c.CreditCardNumber)
                    .IsRequired()
                    .HasMaxLength(19)
                    .IsFixedLength()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasOne(sa => sa.Store)
                    .WithMany(st => st.Sales)
                    .HasForeignKey(sa => sa.StoreId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sa => sa.Product)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(sa => sa.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sa => sa.Customer)
                    .WithMany(c => c.Sales)
                    .HasForeignKey(sa => sa.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }

        public void Seed()
        {
            if (Products.Any() || Customers.Any() || Stores.Any() || Sales.Any())
            {
                Console.WriteLine("The DataBase is already seeded!");
                return;
            }

            var productFaker = new Faker<Product>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Quantity, f => (float)Math.Round(f.Random.Double(1, 100), 2))
                .RuleFor(p => p.Price, f => Math.Round(decimal.Parse(f.Commerce.Price(5, 500)), 2));

            var products = productFaker.Generate(25);

            var customerFaker = new Faker<Customer>()
                .RuleFor(c => c.Name, f => f.Person.FullName)
                .RuleFor(c => c.Email, (f, c)=> f.Internet.Email(c.Name).ToLower())
                .RuleFor(c => c.CreditCardNumber, f => "1234567898765432");

            var customer = customerFaker.Generate(25);

            var storeFaker = new Faker<Store>()
                .RuleFor(s => s.Name, f => f.Company.CompanyName());

            var store = storeFaker.Generate(25);

            var saleFaker = new Faker<Sale>()
                .RuleFor(s => s.Date, f => f.Date.Past(1))
                .RuleFor(s => s.Product, f => f.PickRandom(products))
                .RuleFor(s => s.Customer, f => f.PickRandom(customer))
                .RuleFor(s => s.Store, f => f.PickRandom(store));

            var sales = saleFaker.Generate(60);

            Products.AddRange(products);
            Customers.AddRange(customer);
            Stores.AddRange(store);
            Sales.AddRange(sales);

            SaveChanges();


        }
    }
}
