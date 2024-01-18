using Microsoft.EntityFrameworkCore;
using Reservation.Entities;

namespace Reservation
{
    public class ReservationDbContext : DbContext
    {
        public DbSet<Reservations> Reservations { get; set; }
        public ReservationDbContext()
        {
            Database.EnsureCreated();
        }
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservations>(entity =>
            {
                entity.ToTable("reservations");

                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Reservation_uid, "reservation_reservation_uid_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("username")
                    .HasMaxLength(80)
                    .IsRequired();

                entity.Property(e => e.Book_uid)
                    .HasColumnName("book_uid")
                    .HasColumnType("uuid")
                    .IsRequired();

                entity.Property(e => e.Library_uid)
                    .HasColumnName("library_uid")
                    .HasColumnType("uuid")
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.ToTable("reservations", t => t.HasCheckConstraint("check_status", "status IN ('RENTED', 'RETURNED', 'EXPIRED')"));

                entity.Property(e => e.Start_date)
                    .HasColumnName("start_date")
                    .HasColumnType("timestamp")
                    .IsRequired();

                entity.Property(e => e.Till_date)
                    .HasColumnName("till_date")
                    .HasColumnType("timestamp")
                    .IsRequired();
            });
        }
    }
}
