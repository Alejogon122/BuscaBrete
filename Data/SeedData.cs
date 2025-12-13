using Microsoft.AspNetCore.Identity;
using BuscaBrete.Models;

namespace BuscaBrete.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        { //Se definen los roles
            string[] roles = { "Empresa", "Postulante" };

            //Se crean los roles si no existen
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            //Creamos el usuario administrador (en este caso empresa)
            string adminEmail = "admin@buscabrete.com";
            string adminPassword = "Admin123";

            //Busca si el usuario ya existe
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null) //si no existe, se crea
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    NombreCompleto = "Administrador BuscaBrete",
                    RolDisplay = "Empresa"
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)//Si todo sale bien...
                {
                    //Asignamos el rol de Empresa al usuario administrador
                    await userManager.AddToRoleAsync(adminUser, "Empresa");
                }
            }
        }
    }
}
