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

        public DbSet<Usuario> Student { get; set; }
        public DbSet<Correspondencias> Correspondencia { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<Comunidades> Comunidades { get; set; }
        public DbSet<Importancia> Importancia { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<UsuarioArea> UsuarioArea { get; set; }
        public DbSet<FormularioAreaDestino> FormularioAreaDestino { get; set; }
        public DbSet<Respuesta> Respuesta { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Correspondencias>()
                .HasOne(c => c.AreaNavigation)
                .WithMany(a => a.Correspondencias)
                .HasForeignKey(c => c.Area)
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

            // CONFIGURACIÓN EXPLÍCITA
            modelBuilder.Entity<FormularioAreaDestino>()
                .HasOne(f => f.Correspondencia)
                .WithMany()
                .HasForeignKey(f => f.CorrespondenciaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FormularioAreaDestino>()
                .HasOne(f => f.Area)
                .WithMany()
                .HasForeignKey(f => f.AreaId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
