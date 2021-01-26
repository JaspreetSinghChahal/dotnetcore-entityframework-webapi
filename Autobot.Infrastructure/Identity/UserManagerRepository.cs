using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autobot.Infrastructure.Identity
{
    public class UserManagerRepository : IUserManagerRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        readonly RoleManager<IdentityRole> _roleManager;
        public UserManagerRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Add a new user
        /// Add user details and roles
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        public async Task<string> CreateUserAsync(RegisterUser register)
        {
            var user = new ApplicationUser
            {
                UserName = register.PhoneNumber,
                FirstName = register.FirstName,
                LastName = register.LastName,
                Location = register.Location,
                OtherDetails = register.OtherDetails,
                PhoneNumber = register.PhoneNumber,
                DisplayPassword = register.Password,
                Email = register.Email,
                EmailConfirmed = !string.IsNullOrEmpty(register.Email)
            };
            var result = await _userManager.CreateAsync(user, register.Password);
            if (result.Succeeded)
            {
                AddUserToRole(register.RoleName, user);
            }
            return user.Id;
        }

        /// <summary>
        /// Get users with given user name
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetUserDetailByUserName(string username)
        {
            var result = await _userManager.FindByNameAsync(username);
            return result;
        }

        /// <summary>
        /// Get All users
        /// </summary>
        /// <returns></returns>
        public List<ApplicationUser> GetUserList()
        {
            var result = _userManager.Users;
            return result.ToList();
        }

        /// <summary>
        /// Get details of a user
        /// </summary>
        /// <param name="id">Id of user</param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetUserInfoById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }


        public async Task<List<ApplicationUser>> GetUsersByIds(List<string> ids)
        {
            var result = _userManager.Users.Where(x => ids.Any(y => y == x.Id));
            return await Task.FromResult(result.ToList());
        }
        /// <summary>
        /// Update info of existsing user
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> ResetPasswordAsync(ApplicationUser user, string password)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            return result.Succeeded;
        }


        public async Task<bool> ResetPasswordWithTokenAsync(ApplicationUser user, string password, string token)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            return result.Succeeded;
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            return false;
        }


        public async Task<bool> DeleteUsersAsync(List<string> userIds)
        {
            bool succeeded = false;
            var users = _userManager.Users.Where(u => userIds.Contains(u.Id)).ToList();
            if (users != null)
            {
                foreach (var user in users)
                {
                    var result = await _userManager.DeleteAsync(user);
                    succeeded = result.Succeeded;
                }
                return succeeded;
            }
            return succeeded;
        }

        /// <summary>
        /// Check password
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> CheckPassword(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        /// <summary>
        /// Get all roles of a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IList<string>> GetRoles(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        /// <summary>
        /// Get all roles in application
        /// </summary>
        /// <returns></returns>
        public async Task<List<IdentityRole>> GetAllRoles()
        {
            return await Task.FromResult(_roleManager.Roles.ToList());
        }

        /// <summary>
        /// Assign a role to user
        /// </summary>
        /// <param name="role"></param>
        /// <param name="user"></param>
        public void AddUserToRole(string role, ApplicationUser user)
        {
            var hasRole = _roleManager.RoleExistsAsync(role).GetAwaiter().GetResult();
            if (hasRole)
            {
                _userManager.AddToRoleAsync(user, role).GetAwaiter().GetResult();
            }
        }

        /// <summary>
        /// Remove role of a user
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="user"></param>
        public void RemoveUserFromRoles(List<string> roles, ApplicationUser user)
        {
            _userManager.RemoveFromRolesAsync(user, roles).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Add role in a ystem
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<bool> AddRoles(IdentityRole role)
        {
            var res = await _roleManager.CreateAsync(role);
            return res.Succeeded;
        }

        public async Task<IList<ApplicationUser>> GetUserWithGivenRole(string roleName)
        {
            return await _userManager.GetUsersInRoleAsync(roleName);
        }

        public async Task<ApplicationUser> FindUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<string> GeneratePasswordResetToken(ApplicationUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
    }
}
