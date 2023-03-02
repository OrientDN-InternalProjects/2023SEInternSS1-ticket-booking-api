﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TicketBooking.Data.DbContext;

#nullable disable

namespace TicketBooking.Data.Migrations
{
    [DbContext(typeof(TicketBookingDbContext))]
    partial class TicketBookingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Aircraft", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Manufacture")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<int?>("NumColumnSeat")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("NumRowBusiness")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("NumRowEconomy")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("NumRowSeat")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Aircraft");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Airport", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Airport");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Booking", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ContactId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateBooking")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPaid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsRoundFlight")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int?>("NumberPeople")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("TotalPrice")
                        .IsRequired()
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ContactId")
                        .IsUnique()
                        .HasFilter("[ContactId] IS NOT NULL");

                    b.HasIndex("UserId");

                    b.ToTable("Booking");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.BookingList", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FlightId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("FlightPrice")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int?>("NumberSeat")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.HasIndex("FlightId");

                    b.ToTable("BookingList");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.BookingSeat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookingListId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("PriceSeat")
                        .HasColumnType("decimal(10,2)");

                    b.Property<Guid>("SeatId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BookingListId");

                    b.HasIndex("SeatId")
                        .IsUnique();

                    b.ToTable("BookingSeat");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.ContactDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.HasKey("Id");

                    b.ToTable("ContactDetail");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Flight", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AircraftId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("BusinessPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("DefaultBaggage")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<decimal?>("EconomyPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsFlightActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int?>("RemainingSeat")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<Guid?>("ScheduleId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("TotalSeat")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AircraftId");

                    b.HasIndex("ScheduleId");

                    b.ToTable("Flight");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.FlightSchedule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ArrivalAirportId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ArrivalTime")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DepartureAirportId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DepartureTime")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ArrivalAirportId");

                    b.HasIndex("DepartureAirportId");

                    b.ToTable("FlightSchedule");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Passenger", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExpDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("IdentityCard")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Nation")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ProvideNa")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.ToTable("Passenger");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsReVoke")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("IssuedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("JwtId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Seat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AirCraftId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CoordinateX")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<int>("CoordinateY")
                        .HasColumnType("int");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<Guid>("SeatClassId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SeatCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AirCraftId");

                    b.HasIndex("SeatClassId");

                    b.ToTable("Seat");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.SeatClass", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SeatName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("SeatClass");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AircraftModel")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("AirlineName")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<Guid?>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("LocationFrom")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LocationTo")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<Guid?>("PassengerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PassengerName")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.Property<string>("SeatClass")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.HasIndex("PassengerId")
                        .IsUnique()
                        .HasFilter("[PassengerId] IS NOT NULL");

                    b.ToTable("Ticket");
                });

            modelBuilder.Entity("TicketBooking.Model.DataModel.BookingExtraService", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookingListId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ExtraServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BookingListId");

                    b.HasIndex("ExtraServiceId");

                    b.ToTable("BookingService");
                });

            modelBuilder.Entity("TicketBooking.Model.DataModel.ExtraService", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("NameService")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("Id");

                    b.ToTable("ExtraService");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TicketBooking.Data.DataModel.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TicketBooking.Data.DataModel.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TicketBooking.Data.DataModel.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TicketBooking.Data.DataModel.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Booking", b =>
                {
                    b.HasOne("TicketBooking.Data.DataModel.ContactDetail", "ContactDetail")
                        .WithOne("Booking")
                        .HasForeignKey("TicketBooking.Data.DataModel.Booking", "ContactId");

                    b.HasOne("TicketBooking.Data.DataModel.ApplicationUser", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("UserId");

                    b.Navigation("ContactDetail");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.BookingList", b =>
                {
                    b.HasOne("TicketBooking.Data.DataModel.Booking", "Booking")
                        .WithMany("BookingLists")
                        .HasForeignKey("BookingId");

                    b.HasOne("TicketBooking.Data.DataModel.Flight", "Flight")
                        .WithMany("BookingLists")
                        .HasForeignKey("FlightId");

                    b.Navigation("Booking");

                    b.Navigation("Flight");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.BookingSeat", b =>
                {
                    b.HasOne("TicketBooking.Data.DataModel.BookingList", "BookingList")
                        .WithMany("ListSeats")
                        .HasForeignKey("BookingListId")
                        .IsRequired();

                    b.HasOne("TicketBooking.Data.DataModel.Seat", "Seat")
                        .WithOne("ListSeat")
                        .HasForeignKey("TicketBooking.Data.DataModel.BookingSeat", "SeatId")
                        .IsRequired();

                    b.Navigation("BookingList");

                    b.Navigation("Seat");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Flight", b =>
                {
                    b.HasOne("TicketBooking.Data.DataModel.Aircraft", "Aircraft")
                        .WithMany("Flights")
                        .HasForeignKey("AircraftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TicketBooking.Data.DataModel.FlightSchedule", "Schedule")
                        .WithMany("Flights")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aircraft");

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.FlightSchedule", b =>
                {
                    b.HasOne("TicketBooking.Data.DataModel.Airport", "AirportArrival")
                        .WithMany("ArrivalAirports")
                        .HasForeignKey("ArrivalAirportId")
                        .IsRequired();

                    b.HasOne("TicketBooking.Data.DataModel.Airport", "AirportDepart")
                        .WithMany("DepartureAirports")
                        .HasForeignKey("DepartureAirportId")
                        .IsRequired();

                    b.Navigation("AirportArrival");

                    b.Navigation("AirportDepart");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Passenger", b =>
                {
                    b.HasOne("TicketBooking.Data.DataModel.Booking", "Booking")
                        .WithMany("Passengers")
                        .HasForeignKey("BookingId");

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.RefreshToken", b =>
                {
                    b.HasOne("TicketBooking.Data.DataModel.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Seat", b =>
                {
                    b.HasOne("TicketBooking.Data.DataModel.Aircraft", "Aircraft")
                        .WithMany("Seats")
                        .HasForeignKey("AirCraftId")
                        .IsRequired();

                    b.HasOne("TicketBooking.Data.DataModel.SeatClass", "SeatClass")
                        .WithMany("Seats")
                        .HasForeignKey("SeatClassId")
                        .IsRequired();

                    b.Navigation("Aircraft");

                    b.Navigation("SeatClass");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Ticket", b =>
                {
                    b.HasOne("TicketBooking.Data.DataModel.Booking", "Booking")
                        .WithMany("Tickets")
                        .HasForeignKey("BookingId");

                    b.HasOne("TicketBooking.Data.DataModel.Passenger", "Passenger")
                        .WithOne("Ticket")
                        .HasForeignKey("TicketBooking.Data.DataModel.Ticket", "PassengerId");

                    b.Navigation("Booking");

                    b.Navigation("Passenger");
                });

            modelBuilder.Entity("TicketBooking.Model.DataModel.BookingExtraService", b =>
                {
                    b.HasOne("TicketBooking.Data.DataModel.BookingList", "BookingList")
                        .WithMany("BookingServices")
                        .HasForeignKey("BookingListId")
                        .IsRequired();

                    b.HasOne("TicketBooking.Model.DataModel.ExtraService", "ExtraService")
                        .WithMany("BookingServices")
                        .HasForeignKey("ExtraServiceId")
                        .IsRequired();

                    b.Navigation("BookingList");

                    b.Navigation("ExtraService");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Aircraft", b =>
                {
                    b.Navigation("Flights");

                    b.Navigation("Seats");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Airport", b =>
                {
                    b.Navigation("ArrivalAirports");

                    b.Navigation("DepartureAirports");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.ApplicationUser", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Booking", b =>
                {
                    b.Navigation("BookingLists");

                    b.Navigation("Passengers");

                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.BookingList", b =>
                {
                    b.Navigation("BookingServices");

                    b.Navigation("ListSeats");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.ContactDetail", b =>
                {
                    b.Navigation("Booking");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Flight", b =>
                {
                    b.Navigation("BookingLists");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.FlightSchedule", b =>
                {
                    b.Navigation("Flights");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Passenger", b =>
                {
                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.Seat", b =>
                {
                    b.Navigation("ListSeat");
                });

            modelBuilder.Entity("TicketBooking.Data.DataModel.SeatClass", b =>
                {
                    b.Navigation("Seats");
                });

            modelBuilder.Entity("TicketBooking.Model.DataModel.ExtraService", b =>
                {
                    b.Navigation("BookingServices");
                });
#pragma warning restore 612, 618
        }
    }
}
