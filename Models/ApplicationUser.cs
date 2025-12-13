using Microsoft.AspNetCore.Identity;

namespace BuscaBrete.Models
{
    //Heredamos de IdentityUser para extender la información del usuario
    public class ApplicationUser : IdentityUser
    {
        public string? NombreEmpresa { get; set; }
        public string? NombreCompleto { get; set; }
        public string? RolDisplay { get; set; } //Usamos solo para mostrar en la interfaz el rol del usuario "Empresa" ó "Postulante"
        public PerfilPostulante? Perfil { get; set; } //Relación 1:1 con PerfilPostulante ya q un usuario puede tener o no perfil.
    }
}
