using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data.DataModel;

namespace TicketBooking.Data.DbContext
{
    public class TicketBookingDbContext : IdentityDbContext<ApplicationUser>
    {
        public static TicketBookingDbContext Create()
        {
            return new TicketBookingDbContext();
        }
        public TicketBookingDbContext(DbContextOptions<TicketBookingDbContext> options) : base(options) { }

        public TicketBookingDbContext()
        {
        }
        #region
        public DbSet<Aircraft>? Aircrafts { get; set; }
        public DbSet<Airport>? Airports { get; set; }
        public DbSet<SeatClass>? SeatClasses { get; set; }
        public DbSet<FlightSchedule>? FlightSchedules { get; set; }
        public DbSet<Flight>? Flights { get; set; }
        public DbSet<Seat>? Seats { get; set; }
        public DbSet<ContactDetail>? ContactDetails { get; set; }
        public DbSet<ExtraBaggage>? ExtraBaggages { get; set; }
        public DbSet<Booking>? Bookings { get; set; }
        public DbSet<Passenger>? Passengers { get; set; }
        public DbSet<Ticket>? Tickets { get; set; }
        public DbSet<BookingList>? BookingLists { get; set; }
        public DbSet<ListSeat>? ListSeats { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Aircraft>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Airport>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<FlightSchedule>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.AirportDepart)
                .WithMany(e => e.DepartureAirports)
                .HasForeignKey(e => e.DepartureAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull); 

                entity.HasOne(e => e.AirportArrival)
                .WithMany(e => e.ArrivalAirports)
                .HasForeignKey(e => e.ArrivalAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull); 
            });

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(entity => entity.IsFlightActive).HasDefaultValue("false");

                entity.HasOne(e => e.Schedule)
                .WithMany(e => e.Flights)
                .HasForeignKey(e => e.ScheduleId);
            });

            modelBuilder.Entity<SeatClass>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.SeatClass)
                .WithMany(e => e.Seats)
                .HasForeignKey(e => e.SeatClassId);

                entity.HasOne(e => e.ListSeat)
                .WithOne(e => e.Seat)
                .HasForeignKey<ListSeat>(e => e.SeatId);
            });

            modelBuilder.Entity<ContactDetail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.Booking)
                .WithOne(e => e.ContactDetail)
                .HasForeignKey<Booking>(e => e.ContactId);
            });

            modelBuilder.Entity<ExtraBaggage>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.IsPaid).HasDefaultValue("false");

                entity.Property(e => e.IsRoundFlight).HasDefaultValue("false");

                entity.HasOne(e => e.ExtraBaggage)
                .WithMany(e => e.Bookings)
                .HasForeignKey(e => e.ExtraBaggageId);

                entity.HasOne(e => e.User)
               .WithMany(e => e.Bookings)
               .HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<BookingList>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.Booking)
                .WithMany(e => e.BookingLists)
                .HasForeignKey(e => e.BookingId);

                entity.HasOne(e => e.Flight)
               .WithMany(e => e.BookingLists)
               .HasForeignKey(e => e.FlightId);
            });

            modelBuilder.Entity<ListSeat>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.BookingList)
               .WithMany(e => e.ListSeats)
               .HasForeignKey(e => e.BookingListId)
               .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Passenger>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.Booking)
               .WithMany(e => e.Passengers)
               .HasForeignKey(e => e.BookingId);

                entity.HasOne(e => e.Ticket)
                .WithOne(e => e.Passenger)
                .HasForeignKey<Ticket>(e => e.PassengerId);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.Booking)
               .WithMany(e => e.Tickets)
               .HasForeignKey(e => e.BookingId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            });

        }
        }
}
