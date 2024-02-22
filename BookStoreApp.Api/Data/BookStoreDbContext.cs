using System;
using System.Collections.Generic;
// using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
    // IdentityDbContext is a base class from the ASP.NET Identity framework for managing -
    // user accounts and authentication.
    //<ApiUser> specifies that this context manages users of type ApiUser    
    public partial class BookStoreDbContext : IdentityDbContext<ApiUser>
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
        // Either of these should not be null, which is indicated with null!
        // The word virtual indicates that these properties can be overridden in derived classes.
        // In other words, if you inherit from this class, you can create your own versions of Authors and Books properties.
        public virtual DbSet<Author> Authors { get; set; } = null!; // Map one to one to a C# Object to a table in SQL 
        public virtual DbSet<Book> Books { get; set; } = null!; 

        


        
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
 
        // Overriden the OnModelCreating method from the base class, to make my own implementation
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			// In MyDbContext.OnModelCreating, I call base.OnModelCreating(modelBuilder) to ensure -
            // the base configurations are applied first, followed by your custom changes.
			base.OnModelCreating(modelBuilder);

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

            modelBuilder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Name = "User",
                        NormalizedName = "USER",
                        Id = "1cf7672a-1d45-4eda-8eab-b3c6ee1f6767"
					},
                    new IdentityRole
                    {
                        Name = "Administrator",
                        NormalizedName = "ADMINISTRATOR",
                        Id = "bbd45a0d-f600-4343-b039-302564c70e1a"
					}
                );

            var hasher = new PasswordHasher<ApiUser>();

            modelBuilder.Entity<ApiUser>().HasData(
                    new ApiUser
                    {
                        Id = "4c71a100-e945-419f-ba73-77f5bce0a041",
                        Email = "admin@bookstore.com",
                        NormalizedEmail = "ADMIN@BOOKSTORE.COM",
                        UserName = "admin@bookstore.com",
                        NormalizedUserName = "ADMIN@BOOKSTORE.COM",
                        FirstName = "System",
                        LastName = "Admin",
                        PasswordHash = hasher.HashPassword(null, "P@ssword1")
                    },
                    new ApiUser
                    {
                        Id = "d5f31ae7-bf33-44fe-8ce4-9285a3ea410f",
						Email = "user@bookstore.com",
						NormalizedEmail = "USER@BOOKSTORE.COM",
						UserName = "user@bookstore.com",
						NormalizedUserName = "USER@BOOKSTORE.COM",
						FirstName = "System",
						LastName = "User",
						PasswordHash = hasher.HashPassword(null, "P@ssword1")
					}
                );

            // asigning users to a role ??
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string>
                    {
                        RoleId = "1cf7672a-1d45-4eda-8eab-b3c6ee1f6767",
                        UserId = "d5f31ae7-bf33-44fe-8ce4-9285a3ea410f",
                    },
                    new IdentityUserRole<string>
                    {
						RoleId = "bbd45a0d-f600-4343-b039-302564c70e1a",
						UserId = "4c71a100-e945-419f-ba73-77f5bce0a041"
					}                        
				);

			OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
