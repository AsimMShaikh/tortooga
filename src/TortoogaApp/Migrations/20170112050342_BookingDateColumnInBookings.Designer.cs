using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TortoogaApp.Data;
using TortoogaApp.Models;

namespace TortoogaApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170112050342_BookingDateColumnInBookings")]
    partial class BookingDateColumnInBookings
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
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

                    b.Property<DateTime?>("BookingDate");

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

                    b.Property<string>("CompanyBio");

                    b.Property<string>("ContactPerson");

                    b.Property<string>("Country");

                    b.Property<string>("Email");

                    b.Property<string>("MCDotNumber");

                    b.Property<string>("MobileNumber");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("PostCode");

                    b.Property<string>("SiteUrl");

                    b.Property<string>("State");

                    b.HasKey("CarrierGuid");

                    b.ToTable("Carriers");
                });

            modelBuilder.Entity("TortoogaApp.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("TortoogaApp.Models.CustomerRatings", b =>
                {
                    b.Property<Guid>("RatingId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("BookingID");

                    b.Property<Guid>("CarrierID");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("Rating");

                    b.Property<DateTime?>("RatingAddedOn");

                    b.Property<Guid>("UserId");

                    b.HasKey("RatingId");

                    b.HasIndex("BookingID");

                    b.ToTable("CustomerRatings");
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

            modelBuilder.Entity("TortoogaApp.Models.ProfileImage", b =>
                {
                    b.Property<Guid>("ImageGuid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContentType");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("Extension");

                    b.Property<string>("ImageUri");

                    b.Property<Guid>("UserId");

                    b.Property<byte[]>("bytes");

                    b.Property<bool>("isDeleted");

                    b.Property<long>("size");

                    b.HasKey("ImageGuid");

                    b.HasIndex("UserId");

                    b.ToTable("ProfileImages");
                });

            modelBuilder.Entity("TortoogaApp.Models.ProvinceState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CountryId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("ProvinceStates");
                });

            modelBuilder.Entity("TortoogaApp.Models.Rating", b =>
                {
                    b.Property<Guid>("RatingId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("BookingID");

                    b.Property<Guid>("CarrierID");

                    b.Property<int>("Communication");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsRequestedForRemoval");

                    b.Property<int>("OverallExperience");

                    b.Property<int>("Price");

                    b.Property<DateTime?>("RatingAddedOn");

                    b.Property<int>("Service");

                    b.Property<Guid>("UserId");

                    b.HasKey("RatingId");

                    b.HasIndex("BookingID");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("TortoogaApp.Security.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<DateTime?>("BirthDate");

                    b.Property<Guid?>("CarrierGuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Country");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("MobileNumber");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("PostCode");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("State");

                    b.Property<string>("Suburb");

                    b.Property<string>("TimeZoneId");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

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
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
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

            modelBuilder.Entity("TortoogaApp.Models.CustomerRatings", b =>
                {
                    b.HasOne("TortoogaApp.Models.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TortoogaApp.Models.Listing", b =>
                {
                    b.HasOne("TortoogaApp.Models.Carrier", "Carrier")
                        .WithMany("Listings")
                        .HasForeignKey("CarrierGuid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TortoogaApp.Models.ProfileImage", b =>
                {
                    b.HasOne("TortoogaApp.Security.ApplicationUser", "ImageForUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TortoogaApp.Models.ProvinceState", b =>
                {
                    b.HasOne("TortoogaApp.Models.Country", "Country")
                        .WithMany("ProvinceStates")
                        .HasForeignKey("CountryId");
                });

            modelBuilder.Entity("TortoogaApp.Models.Rating", b =>
                {
                    b.HasOne("TortoogaApp.Models.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingID")
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
