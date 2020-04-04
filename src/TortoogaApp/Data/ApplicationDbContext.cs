using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using TortoogaApp.Models;
using TortoogaApp.Security;

namespace TortoogaApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Listing> Listings { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Carrier> Carriers { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Country> Countries { get; set; }
        public DbSet<ProvinceState> ProvinceStates { get; set; }

        public DbSet<ProfileImage> ProfileImages { get; set; }

        public DbSet<CarrierRegistration> CarrierRegistrations { get; set; }

        public DbSet<CarrierBankingDetails> CarrierBankingDetails { get; set; }

        public DbSet<CompanyLogo> CompanyLogos { get; set; }

 
        
        public DbSet<CustomerRatings> CustomerRatings { get; set; }

        public DbSet<ShipmentStatus> ShipmentStatus { get; set; }

        public DbSet<Notifications> Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //This is to customize ef using Guid as Primary Key instead of int
            builder.Entity<ApplicationUser>(b =>
            {
                b.Property(u => u.Id).HasDefaultValueSql("newsequentialid()");
            });

            builder.Entity<Role>(b =>
            {
                b.Property(u => u.Id).HasDefaultValueSql("newsequentialid()");
            });

            builder.Entity<Listing>(b =>
            {
                b.Property(u => u.Status).HasDefaultValue(ListingStatus.Active);
            });

            builder.Entity<Carrier>()
                .HasKey(c => c.CarrierGuid);
        }
    }
}