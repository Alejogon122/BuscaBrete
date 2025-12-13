using System;
using System.ComponentModel.DataAnnotations;

namespace BuscaBrete.Models
{
    public class Postulacion
    {
        public int Id { get; set; }

        public DateTime FechaPostulacion { get; set; } = DateTime.UtcNow;

        [Required]
        public string Estado { get; set; } = "Pendiente";

        [Required]
        public string PostulanteId { get; set; }
        public ApplicationUser Postulante { get; set; }

        [Required]
        public int OfertaId { get; set; }
        public Oferta Oferta { get; set; }

        [StringLength(2000)]
        public string? Mensaje { get; set; }
    }
}
