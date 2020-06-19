using JobScheduler.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobScheduler.Controllers
{
    public class UserMethods
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserMethods(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Returns all the users with their assigned role.
        /// </summary>
        /// <returns>Returns a IEnumerable of UserWithRole objects. Each object contains the User object and its Role</returns>
        public async Task<IEnumerable<UserWithRole>> GetUsersAsync()
        {
            List<UserWithRole> result = new List<UserWithRole>();
            try
            {
                List<IdentityUser> users = await _userManager.Users.ToListAsync();
                foreach (IdentityUser user in users)
                {
                    IList<string> roles = await _userManager.GetRolesAsync(user);
                    result.Add(new UserWithRole() { User = user, Role = roles.FirstOrDefault() });
                }

            }
            catch { }

            return result;
        }

        /// <summary>
        /// Returns a user given its id. Returns null if not found.
        /// </summary>
        /// <param name="id">The id of the user</param>
        public async Task<UserWithRole> GetUserByIdAsync(string id)
        {
            try
            {
                IdentityUser user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    IList<string> roles = await _userManager.GetRolesAsync(user);
                    UserWithRole result = new UserWithRole() { User = user, Role = roles.FirstOrDefault() };

                    return result;
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="newUser">A UserWithRole object containing the new user details</param>
        /// <returns>Returns true if the user has been created successfully</returns>
        public async Task<bool> CreateUserAsync(UserWithRole newUser)
        {
            IdentityResult result = await _userManager.CreateAsync(newUser.User, newUser.User.PasswordHash);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser.User, newUser.Role);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Edits an existing user
        /// </summary>
        /// <param name="id">The id of the user to modify</param>
        /// <param name="modifiedUser">A UserWithRole object containing the modified user details</param>
        /// <returns>Returns the modified user object if successfull</returns>
        public async Task<UserWithRole> EditUserAsync(string id, UserWithRole modifiedUser)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                //Update the email
                if (user.Email != modifiedUser.User.Email)
                {
                    user.Email = modifiedUser.User.Email;
                    await _userManager.UpdateAsync(user);
                }

                //The password has been changed
                if (user.PasswordHash != modifiedUser.User.PasswordHash)
                {
                    string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, resetToken, modifiedUser.User.PasswordHash);
                }

                //Role has been changed
                if (!await _userManager.IsInRoleAsync(user, modifiedUser.Role))
                {
                    //Remove old role
                    await _userManager.RemoveFromRoleAsync(user, (await _userManager.GetRolesAsync(user)).FirstOrDefault());
                    //Apply new one
                    await _userManager.AddToRoleAsync(user, modifiedUser.Role);
                }

                return new UserWithRole() { User = user, Role = modifiedUser.Role };
            }

            return null;
        }

        /// <summary>
        /// Deletes the specified user
        /// </summary>
        /// <param name="id">The id of the user to delete</param>
        /// <returns>Returns true is the user has been deleted</returns>
        public async Task<bool?> DeleteUserAsync(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult deleteResult = await _userManager.DeleteAsync(user);
                return deleteResult.Succeeded;
            }
            return null;
        }
    }
}
