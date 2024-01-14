using Microsoft.EntityFrameworkCore;
using Rating.Entities;

namespace Rating
{
    public class RatingDbContext : DbContext
    {
        public DbSet<Ratings> Ratings { get; set; }
        public RatingDbContext()
        {
            Database.EnsureCreated();
        }
        public RatingDbContext(DbContextOptions<RatingDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ratings>(entity =>
            {
                entity.ToTable("ratings");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserName)
                    .HasColumnName("username")
                    .HasMaxLength(80)
                    .IsRequired();

                entity.Property(e => e.Stars)
                    .HasColumnName("stars")
                    .HasDefaultValue(100)
                    .IsRequired();

                entity.ToTable("ratings", t => t.HasCheckConstraint("CK_Rating_Stars", "stars BETWEEN 0 AND 100"));


            });
        }
    }
}