using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autobot.Infrastructure.Identity
{
    public interface IUserManagerRepository
    {
        List<ApplicationUser> GetUserList();
        Task<ApplicationUser> GetUserDetailByUserName(string username);
        Task<ApplicationUser> GetUserInfoById(string id);
        Task<string> CreateUserAsync(RegisterUser register);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> CheckPassword(ApplicationUser user, string password);
        Task<bool> ResetPasswordAsync(ApplicationUser user, string password);
        Task<List<IdentityRole>> GetAllRoles();
        Task<IList<string>> GetRoles(ApplicationUser user);
        Task<bool> AddRoles(IdentityRole role);
        void RemoveUserFromRoles(List<string> roles, ApplicationUser user);
        void AddUserToRole(string role, ApplicationUser user);
        Task<List<ApplicationUser>> GetUsersByIds(List<string> ids);
        Task<IList<ApplicationUser>> GetUserWithGivenRole(string roleName);
        Task<bool> DeleteUsersAsync(List<string> userId);
        Task<ApplicationUser> FindUserByEmail(string email);
        Task<string> GeneratePasswordResetToken(ApplicationUser user);
        Task<bool> ResetPasswordWithTokenAsync(ApplicationUser user, string password, string token);
    }
}