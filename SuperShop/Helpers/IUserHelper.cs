using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using SuperShop.Models;

namespace SuperShop.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        //verifica se ele tem o role, se não, cria
        Task CheckRoleAsync(string roleName);

        //adiciona role a um user
        Task AddUserToRoleAsync(User user, string roleName);

        //Checa se o user já tem um role
        Task<bool> IsUserInRoleAsync(User user, string roleName);
    }
}
