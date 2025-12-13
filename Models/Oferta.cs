using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuscaBrete.Models
{
    public class Oferta
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200)]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Debes agregar los requisitos del puesto")]
        public string Requisitos { get; set; }
        [Required]
        public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;
        [Required(ErrorMessage = "Debe indicar la ubicación del trabajo")]
        [StringLength(200)]
        public string Ubicacion { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "El salario debe ser un valor positivo")]
        public decimal? Salario { get; set; }
        [Required]
        public string EmpresaId { get; set; }
        public ApplicationUser Empresa { get; set; }
    }
}
