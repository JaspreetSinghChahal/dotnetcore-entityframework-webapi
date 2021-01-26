using Autobot.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Autobot.Commands.SystemSeedData
{
    public class InitialDataSeeder
    {
        private readonly IUserManagerRepository _userManager;
        public InitialDataSeeder(IUserManagerRepository userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedAllAsync()
        {
            await SeedRolesAsync();
            await SeedAdminUserAsync();
        }

        private async Task SeedRolesAsync()
        {
            var roles = await _userManager.GetAllRoles();
            string[] newRoles = new string[] { "Admin", "User" };
            foreach (string role in newRoles)
            {
                if (!roles.Any(r => r.Name == role))
                {
                    await _userManager.AddRoles(new IdentityRole(role));
                }
            }
        }

        private async Task SeedAdminUserAsync()
        {
            var registerAdmin = new RegisterUser()
            {
                PhoneNumber = "123456789",
                FirstName = "Jaspreet",
                LastName = "Singh",
                Password = "abc@123",
                Location = "abc",
                OtherDetails = "ss",
                RoleName = "Admin",
                Email = "Jaspreetchahal89@gmail.com"
            };
            var user = await _userManager.GetUserDetailByUserName("123456789");
            if (user == null)
            {
                await _userManager.CreateUserAsync(registerAdmin);
            }
        }
    }
}
