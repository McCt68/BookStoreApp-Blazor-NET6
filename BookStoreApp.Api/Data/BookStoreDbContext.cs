using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// The DB context class is there for EntityFramework to configure the connection to the database ??
// it typically belongs in the Data folder

// This class has two constructors, one default, other initializes some options which it pass over to the DbContext

// DbContext here basically says this class becomes an entity framework recognized class representing -
// The database that we are connecting too. ( I need to understand this better ) represeting the databaseConnection.
// Maybe the explenation is simply that this Inherits from DBContext

namespace BookStoreApp.Api.Data
{
    public partial class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext() // Default Constructor
        {
        }

        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) 
            : base(options) // Constructor pass options to DbContext
        {
        }

        // In summary, these properties define two collections of entities, Authors and Books -
        // which can be used to access and manage author and book data in an Entity Framework Core application.
        // and they should not be null, which is indicated with null!
        // The word virtual indicates that these properties can be overridden in derived classes.
        // In other words, if you inherit from this class, you can create your own versions of Authors and Books properties.
        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;

        // From Code first aproach. See the CodeFirstTable class video 16
        // public virtual DbSet<CodeFirstTable> CodeFirstTables { get; set; } = null!;


        // All this can be removed
        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress;Database=BookStoreDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Bio).HasMaxLength(250);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EA74D763A4")
                    .IsUnique();

                entity.Property(e => e.Image).HasMaxLength(50);

                entity.Property(e => e.Isbn)
                    .HasMaxLength(50)
                    .HasColumnName("ISBN");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Summary).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK_Books_ToTable");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
