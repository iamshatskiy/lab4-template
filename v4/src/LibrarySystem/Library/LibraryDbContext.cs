using Library.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library
{
    public class LibraryDbContext : DbContext
    {

        public DbSet<LibraryBooks> LibraryBooks { get; set; }
        public DbSet<Libraries> Libraries { get; set; }
        public DbSet<Books> Books { get; set; }
        public LibraryDbContext()
        {
            Database.EnsureCreated();
        }
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libraries>(entity =>
            {
                entity.ToTable("library");

                entity.HasIndex(e => e.Library_uid, "library_library_uid_key")
                    .IsUnique();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Library_uid).HasColumnName("library_uid");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .HasColumnName("name")
                    .IsRequired();

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .HasColumnName("city")
                    .IsRequired();

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("address")
                    .IsRequired();

                entity.HasData(
                    new Libraries
                    {
                        Id = 1,
                        Library_uid = Guid.Parse("83575e12-7ce0-48ee-9931-51919ff3c9ee"),
                        Address = "2-я Бауманская ул., д.5, стр.1",
                        City = "Москва",
                        Name = "Библиотека имени 7 Непьющих"
                    });
            });

            modelBuilder.Entity<Books>(entity =>
            {
                entity.ToTable("books");

                entity.HasIndex(e => e.Book_uid, "books_book_uid_key")
                    .IsUnique();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Book_uid).HasColumnName("book_uid");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name")
                    .IsRequired();

                entity.Property(e => e.Author)
                    .HasMaxLength(255)
                    .HasColumnName("author");

                entity.Property(e => e.Genre)
                    .HasMaxLength(255)
                    .HasColumnName("genre");

                entity.Property(e => e.Condition)
                    .HasMaxLength(20)
                    .HasColumnName("condition")
                    .HasDefaultValue("EXCELLENT")
                    .IsRequired();

                entity.ToTable("books", t => t.HasCheckConstraint("condition_check", "condition IN ('EXCELLENT', 'GOOD', 'BAD')"));

                entity.HasData(new Books
                {
                    Id = 1,
                    Book_uid = Guid.Parse("f7cdc58f-2caf-4b15-9727-f89dcc629b27"),
                    Author = "Бьерн Страуструп",
                    Name = "Краткий курс C++ в 7 томах",
                    Condition = "EXCELLENT",
                    Genre = "Научная фантастика"
                });
            });

            modelBuilder.Entity<LibraryBooks>(entity =>
            {
                entity.ToTable("library_books");

                entity.HasKey(e => new { e.Book_id, e.Library_id });

                entity.Property(e => e.Book_id)
                    .HasColumnName("book_id");

                entity.Property(e => e.Library_id)
                    .HasColumnName("library_id");

                entity.Property(e => e.Available_count)
                    .HasColumnName("available_count")
                    .IsRequired();

                entity.HasOne(d => d.Book)
                    .WithMany()
                    .HasForeignKey(d => d.Book_id)
                    .HasConstraintName("library_books_book_id_fkey");

                entity.HasOne(d => d.Library)
                    .WithMany()
                    .HasForeignKey(d => d.Library_id)
                    .HasConstraintName("library_books_library_id_fkey");

                entity.HasData(new LibraryBooks
                {
                    Book_id = 1,
                    Library_id = 1,
                    Available_count = 1
                });
            });



        }
    }
}
