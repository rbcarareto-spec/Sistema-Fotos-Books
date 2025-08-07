using Microsoft.EntityFrameworkCore;
using FotoManager.Models;

namespace FotoManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tabela Books
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            // Tabela Photos
            modelBuilder.Entity<Photo>(entity =>
            { 
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Title).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.UploadDate).IsRequired();
                entity.Property(e => e.Filter).HasMaxLength(50);
                entity.Property(e => e.Frame).HasMaxLength(50);
            });

            // Relacionamento: Photo → Book
            modelBuilder.Entity<Photo>()
                .HasOne(p => p.Book)
                .WithMany(b => b.Photos)
                .HasForeignKey(p => p.BookId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índice para melhorar performance de busca por BookId
            modelBuilder.Entity<Photo>()
                .HasIndex(p => p.BookId);
        }
    }
}