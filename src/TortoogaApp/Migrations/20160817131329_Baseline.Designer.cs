using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TortoogaApp.Data;

namespace TortoogaApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20160817131329_Baseline")]
    partial class Baseline
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TortoogaApp.Models.Booking", b =>
                {
                    b.Property<Guid>("BookingGuid")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("BookedHeight");

                    b.Property<double>("BookedLength");

                    b.Property<double>("BookedWidth");

                    b.Property<decimal>("BookingAmount");

                    b.Property<Guid>("CarrierGuid");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("ListingGuid");

                    b.Property<string>("PaymentReferenceNumber");

                    b.Property<string>("ReferenceNumber");

                    b.Property<int>("Status");

                    b.Property<Guid>("UserId");

                    b.HasKey("BookingGuid");

                    b.HasIndex("ListingGuid");

                    b.HasIndex("UserId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("TortoogaApp.Models.Carrier", b =>
                {
                    b.Property<Guid>("CarrierGuid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountNumber");

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<string>("BusinessName");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("Email");

                    b.Property<string>("MobileNumber");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("PostCode");

                    b.Property<string>("SiteUrl");

                    b.Property<string>("State");

                    b.HasKey("CarrierGuid");

                    b.ToTable("Carriers");
                });

            modelBuilder.Entity("TortoogaApp.Models.Listing", b =>
                {
                    b.Property<Guid>("ListingGuid")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AppraisalDateTime");

                    b.Property<Guid>("CarrierGuid");

                    b.Property<string>("ContactDetails");

                    b.Property<DateTime>("DepartureDate");

                    b.Property<string>("Description");

                    b.Property<string>("Destination");

                    b.Property<string>("DropOffAddress");

                    b.Property<DateTime>("EstimatedArrivalDate");

                    b.Property<double>("Height");

                    b.Property<bool>("IsDeleted");

                    b.Property<double>("Length");

                    b.Property<string>("Origin");

                    b.Property<string>("PickUpAddress");

                    b.Property<decimal>("Price");

                    b.Property<string>("ReferenceNumber");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.Property<string>("Title");

                    b.Property<int>("TransitStops");

                    b.Property<double>("Width");

                    b.HasKey("ListingGuid");

                    b.HasIndex("CarrierGuid");

                    b.ToTable("Listings");
                });

            modelBuilder.Entity("TortoogaApp.Security.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<int>("AccessFailedCount");

                    b.Property<Guid?>("CarrierGuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("CarrierGuid");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TortoogaApp.Security.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("TortoogaApp.Security.Role")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("TortoogaApp.Security.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("TortoogaApp.Security.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("TortoogaApp.Security.Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TortoogaApp.Security.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TortoogaApp.Models.Booking", b =>
                {
                    b.HasOne("TortoogaApp.Models.Listing", "Listing")
                        .WithMany()
                        .HasForeignKey("ListingGuid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TortoogaApp.Security.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TortoogaApp.Models.Listing", b =>
                {
                    b.HasOne("TortoogaApp.Models.Carrier", "Carrier")
                        .WithMany("Listings")
                        .HasForeignKey("CarrierGuid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TortoogaApp.Security.ApplicationUser", b =>
                {
                    b.HasOne("TortoogaApp.Models.Carrier", "Carrier")
                        .WithMany()
                        .HasForeignKey("CarrierGuid");
                });
        }
    }
}
