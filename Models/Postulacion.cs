using System;
using System.ComponentModel.DataAnnotations;

namespace BuscaBrete.Models
{
    public class Postulacion
    {
        public int Id { get; set; }
        public DateTime FechaPostulacion { get; set; }
        [Required]
        public string Estado { get; set; } = "Pendiente";
        [Required]
        public string PostulanteId { get; set; }
        [Required]
        public int OfertaId { get; set; }
    }
}
