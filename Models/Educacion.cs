using System;
using System.ComponentModel.DataAnnotations;

namespace BuscaBrete.Models
{
    public class Educacion
    {
        public int Id { get; set; }

        [Required]
        public int PerfilPostulanteId { get; set; }
        public PerfilPostulante PerfilPostulante { get; set; } = null!;

        [Required, StringLength(200)]
        public string Institucion { get; set; } = null!;

        [StringLength(200)]
        public string? Titulo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaInicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; }

        [StringLength(2000)]
        public string? Descripcion { get; set; }
    }
}
