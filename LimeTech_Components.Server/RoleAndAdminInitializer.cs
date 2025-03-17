namespace LimeTech_Components.Server
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;
    using LimeTech_Components.Server.Data.Models;

    public interface IRoleAndAdminInitializer
    {
        Task InitializeRolesAndAdminAsync();
    }

    public class RoleAndAdminInitializer : IRoleAndAdminInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Customer> _userManager;

        public RoleAndAdminInitializer(RoleManager<IdentityRole> roleManager, UserManager<Customer> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task InitializeRolesAndAdminAsync()
        {
            var adminRole = "Admin";
            if (!await _roleManager.RoleExistsAsync(adminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            var adminEmail = "admin@limetech.com";
            var adminPassword = "Admin123!";
            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new Customer
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Administrator"
                };
                var result = await _userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }
    }

}
