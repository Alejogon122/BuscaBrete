using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BuscaBrete.Models;

namespace BuscaBrete.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> //cambiamos para heredar de IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BuscaBrete.Models.Oferta> Oferta { get; set; } = default!;
        public DbSet<BuscaBrete.Models.Postulacion> Postulacion { get; set; } = default!;
        public DbSet<BuscaBrete.Models.PerfilPostulante> PerfilPostulante { get; set; } = default!; 
        public DbSet<ExperienciaLaboral> ExperienciaLaboral { get; set; } = default!;
        public DbSet<Educacion> Educacion { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Relación 1:1 ApplicationUser - PerfilPostulante
            builder.Entity<PerfilPostulante>()
                .HasOne(p => p.Postulante)
                .WithOne(u => u.Perfil)
                .HasForeignKey<PerfilPostulante>(p => p.PostulanteId)
                .OnDelete(DeleteBehavior.Cascade); //Si se borra el usuario, se borra el perfil también

            //Index para búsquedas del postulante
            builder.Entity<PerfilPostulante>()
                .HasIndex(p => p.PostulanteId) //validacion para asegurar que un postulante tenga un solo perfil
                .IsUnique();

            // Relación Oferta (1) -> Postulacion (N), cascada al eliminar oferta
            builder.Entity<Oferta>()
                .HasMany(o => o.Postulaciones)
                .WithOne(p => p.Oferta)
                .HasForeignKey(p => p.OfertaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación ApplicationUser (Postulante) (1) -> Postulacion (N), usando FK PostulanteId (string)
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Postulaciones)
                .WithOne(p => p.Postulante)
                .HasForeignKey(p => p.PostulanteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
