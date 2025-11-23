namespace Invoices.Data
{
    using Invoices.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    using static Common.EntityValidation;

    public class InvoicesContext : DbContext
    {
        public InvoicesContext() 
        { 

        }

        public InvoicesContext(DbContextOptions options)
            : base(options)
        { 

        }

        public virtual DbSet<Product> Products { get; set; } = null!;

        public virtual DbSet<Client> Clients { get; set; } = null!;

        public virtual DbSet<Address> Addresses { get; set; } = null!;

        public virtual DbSet<Invoice> Invoices { get; set; } = null!;

        public virtual DbSet<ProductClient> ProductsClients { get; set; } = null!;

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
            modelBuilder.Entity<Product>()
                .HasCheckConstraint("CK_Product_NameLength",
                $"LEN([Name]) >= {MinProductNameLength} AND LEN([Name]) <= {MaxProductNameLength}");

            modelBuilder.Entity<Product>()
                .HasCheckConstraint("CK_Product_PriceRange",
                $"[Price] >= {MinProductPriceRange} AND [Price] <= {MaxProductPriceRange}");

            modelBuilder.Entity<Address>()
                .HasCheckConstraint("CK_Address_StreetNameLength",
                $"LEN([StreetName]) >= {MinAddressStreetNameLength} AND LEN([StreetName]) <= {MaxAddressStreetNameLength}");

            modelBuilder.Entity<Address>()
                .HasCheckConstraint("CK_Address_CityLength",
                $"LEN([City]) >= {MinAddressCityLength} AND LEN([City]) <= {MaxAddressCityLength}");

            modelBuilder.Entity<Address>()
                .HasCheckConstraint("CK_Address_CountryLength",
                $"LEN([Country]) >= {MinAddressCountryLength} AND LEN([Country]) <= {MaxAddressCountryLength}");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasOne(a => a.Client)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Invoice>()
                .HasCheckConstraint("CK_Invoice_NumberRange",
                $"[Number] >= {MinInvoiceNumberRange} AND [Number] <= {MaxInvoiceNumberRange}");

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasOne(i => i.Client)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Client>()
                .HasCheckConstraint("CK_Client_NameLength",
                $"LEN([Name]) >= {MinClientNameLength} AND LEN([Name]) <= {MaxClientNameLength}");

            modelBuilder.Entity<Client>()
                .HasCheckConstraint("CK_Client_NumberVatLength",
                $"LEN([NumberVat]) >= {MinClientNumberVatLength} AND LEN([NumberVat]) <= {MaxClientNumberVatLength}");

            modelBuilder.Entity<ProductClient>()
                .HasKey(pc => new { pc.ProductId, pc.ClientId });

            modelBuilder.Entity<ProductClient>(entity =>
            {
                entity.HasOne(pc => pc.Product)
                .WithMany(p => p.ProductsClients)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(pc => pc.Client)
                .WithMany(c => c.ProductsClients)
                .HasForeignKey(pc => pc.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
