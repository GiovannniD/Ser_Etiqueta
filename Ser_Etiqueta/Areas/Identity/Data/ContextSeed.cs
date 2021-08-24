using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Areas.Identity.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Facturacion.ToString()));
           // await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Etiqueta.ToString()));
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                FirstName = "SERSA",
                LastName = "SERSA",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "SERSA2021!");
                    await userManager.AddToRoleAsync(defaultUser, Enum.Roles.Etiqueta.ToString());
                   // await userManager.AddToRoleAsync(defaultUser, Enum.Roles.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enum.Roles.Facturacion.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enum.Roles.SuperAdmin.ToString());
                }

            }
        }
    }
}
