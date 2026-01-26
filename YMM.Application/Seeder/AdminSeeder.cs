using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Data.Entities.Identity;

namespace YMM.Application.Seeder
{

    public static class AdminSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider
                .GetRequiredService<UserManager<User>>();

            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            string adminRole = "Admin";
            string superAdminRole = "SuperAdmin";

            if (!await roleManager.RoleExistsAsync(adminRole))
                await roleManager.CreateAsync(new IdentityRole(adminRole));

            if (!await roleManager.RoleExistsAsync(superAdminRole))
                await roleManager.CreateAsync(new IdentityRole(superAdminRole));

            var adminEmail = "admin@ymm.com";
            var adminUserName = "ymm_admin";
            var adminPassword = "Admin@12345"; 
            var adminPhoneNumber = "1234567890";
            var adminCountry = "Egypt";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumber= adminPhoneNumber,
                    Country= adminCountry,
                    Age=30
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (!result.Succeeded)
                    throw new Exception(
                        string.Join(",", result.Errors.Select(e => e.Description))
                    );

                await userManager.AddToRoleAsync(adminUser, superAdminRole);
            }
        }
    }

}
