using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BuscaBrete.Models
{
    /// <summary>
    /// Perfil del postulante (1:1 con ApplicationUser).
    /// Guarda datos personales, CV, habilidades y experiencia y educación.
    /// </summary>
    public class PerfilPostulante
    {
        public int Id { get; set; }
        public string? PostulanteId { get; set; } //FK hacia ApplicationUser
        public ApplicationUser? Postulante { get; set; }
        [StringLength(200)]
        public string? Direccion { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }
        [StringLength(1000)]
        public string? Resumen { get; set; }//Resumen "Acerca de mí"
        [StringLength(100)]
        public string? Telefono { get; set; }
        [StringLength(1000)]
        public string? Habilidades { get; set; }
        public string? CVPath { get; set; }

        //Relaciones 1:1
        public ICollection<ExperienciaLaboral> Experiencias { get; set; } = new List<ExperienciaLaboral>();
        public ICollection<Educacion> Educaciones { get; set; } = new List<Educacion>();
    }
}
