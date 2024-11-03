namespace Properly.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Properly.Common;
    using Properly.Data.Models.User;

    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRoleAsync(roleManager, GlobalConstants.AdministratorRoleName);
            await SeedRoleAsync(roleManager, GlobalConstants.AgencyRoleName);
            await SeedAdministrationAsync(userManager);
        }

        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }

        private static async Task SeedAdministrationAsync(UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.FindByEmailAsync("admin@gmail.com");

            if (user is null)
            {
                var adminUser = new ApplicationUser()
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "Admin",
                    LastName = "Admin",
                    PhoneNumber = "+35911111111"
                };
                string adminPassword = "Adm1nPassw@rd!";

                var createdAdmin = await userManager.CreateAsync(adminUser, adminPassword);
                if (createdAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, GlobalConstants.AdministratorRoleName);
                }
            }
        }
    }
}
