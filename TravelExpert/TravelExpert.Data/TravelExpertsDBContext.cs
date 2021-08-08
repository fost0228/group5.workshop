using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace TravelExpert.Data
{
    public partial class TravelExpertsDBContext : DbContext
    {
        public TravelExpertsDBContext()
            : base("name=TravelExperts")
        {
        }

        public virtual DbSet<Affiliation> Affiliations { get; set; }
        public virtual DbSet<Agency> Agencies { get; set; }
        public virtual DbSet<Agent> Agents { get; set; }
        public virtual DbSet<BookingDetail> BookingDetails { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<CreditCard> CreditCards { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Customers_Rewards> Customers_Rewards { get; set; }
        public virtual DbSet<Fee> Fees { get; set; }
        public virtual DbSet<Package> Packages { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Products_Suppliers> Products_Suppliers { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Reward> Rewards { get; set; }
        public virtual DbSet<SupplierContact> SupplierContacts { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TripType> TripTypes { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Affiliation>()
                .HasMany(e => e.SupplierContacts)
                .WithOptional(e => e.Affiliation)
                .HasForeignKey(e => e.AffiliationId);

            modelBuilder.Entity<BookingDetail>()
                .Property(e => e.BasePrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<BookingDetail>()
                .Property(e => e.AgencyCommission)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Customer>()
                .Property(e => e.CustPwd)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.CreditCards)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Customers_Rewards)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Fee>()
                .Property(e => e.FeeAmt)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Package>()
                .Property(e => e.PkgBasePrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Package>()
                .Property(e => e.PkgAgencyCommission)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Package>()
                .HasMany(e => e.Products_Suppliers)
                .WithMany(e => e.Packages)
                .Map(m => m.ToTable("Packages_Products_Suppliers").MapLeftKey("PackageId").MapRightKey("ProductSupplierId"));

            modelBuilder.Entity<Reward>()
                .HasMany(e => e.Customers_Rewards)
                .WithRequired(e => e.Reward)
                .WillCascadeOnDelete(false);
        }
    }
}
