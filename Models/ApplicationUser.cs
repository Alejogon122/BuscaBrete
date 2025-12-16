using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BuscaBrete.Models
{
    
    public class ApplicationUser : IdentityUser
    {
        public string? NombreEmpresa { get; set; }
        public string? NombreCompleto { get; set; }
        public string? RolDisplay { get; set; } 
        public PerfilPostulante? Perfil { get; set; } 

       
        public ICollection<Postulacion> Postulaciones { get; set; } = new List<Postulacion>();
    }
}
