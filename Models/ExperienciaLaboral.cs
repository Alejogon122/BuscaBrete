using System;
using System.ComponentModel.DataAnnotations;

namespace BuscaBrete.Models
{
    public class ExperienciaLaboral
    {
        public int Id { get; set; }

        [Required]
        public int PerfilPostulanteId { get; set; }

        public PerfilPostulante PerfilPostulante { get; set; } = null!;

        [Required, StringLength(200)]
        public string Puesto { get; set; } = null!;

        [StringLength(200)]
        public string? Empresa { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaInicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; }

        public bool Actualmente { get; set; } = false;

        [StringLength(2000)]
        public string? Descripcion { get; set; }
    }
}
