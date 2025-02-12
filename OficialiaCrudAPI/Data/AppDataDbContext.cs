using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OficialiaCrudAPI.Models;

namespace OficialiaCrudAPI.Data
{
    public class AppDataDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDataDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Usuario> Students { get; set; }
        public DbSet<Correspondencias> Correspondencia { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<Comunidades> Comunidades { get; set; }
        public DbSet<Importancia> Importancia { get; set; }
        public DbSet<Status> Status { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Correspondencias>()
                .HasOne(c => c.AreaNavigation) // Una correspondencia tiene una sola área
                .WithMany(a => a.Correspondencias) // Un área puede tener muchas correspondencias
                .HasForeignKey(c => c.Area) // Llave foránea en Correspondencia
                .HasConstraintName("FK_correspondencia_area");

            modelBuilder.Entity<Correspondencias>()
                .HasOne(c => c.ComunidadNavigation) 
                .WithMany(com => com.Correspondencias) 
                .HasForeignKey(c => c.Comunidad) 
                .HasConstraintName("FK_correspondencia_comunidad");

            modelBuilder.Entity<Correspondencias>()
               .HasOne(c => c.ImportanciaNavigation)
               .WithMany(com => com.Correspondencias)
               .HasForeignKey(c => c.Importancia)
               .HasConstraintName("FK_correspondencia_importancia");

            modelBuilder.Entity<Correspondencias>()
               .HasOne(c => c.StatusNavigation)
               .WithMany(com => com.Correspondencias)
               .HasForeignKey(c => c.Status)
               .HasConstraintName("FK_correspondencia_status");
        }

    }
}
