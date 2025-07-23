using Estatistica.DataAccessLayer.Context;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Estatistica.DataAccessLayer.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public UsuarioRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context=context;
            this.userManager=userManager;
        }

        public async Task<IdentityUser> AddUser(IdentityUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
                return user;
            throw new InvalidDataException(string.Join("\n", result.Errors?.Select(x => x.Description ?? string.Empty) ?? new List<string>()));
        }

        public async Task<bool> ChangeUserStatus(string userId, bool isActive)
        {
            var usuario = await GetUsuarioById(userId);
            if (usuario is not null)
            {
                if (isActive)
                {
                    await userManager.SetLockoutEnabledAsync(usuario, false);
                    await userManager.SetLockoutEndDateAsync(usuario, null);
                }
                else
                {
                    await userManager.SetLockoutEnabledAsync(usuario, true);
                    await userManager.SetLockoutEndDateAsync(usuario, DateTimeOffset.Now.AddYears(1000));
                }
                return true;
            }
            return false;
        }

        public async Task<IdentityUser?> GetUsuarioById(string id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<IdentityUser?> GetUsuarioByUserName(string userName)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<IEnumerable<IdentityUser>> GetUsers()
        {
            return await context.Users.OrderBy(x => x.UserName).ToListAsync();
        }

        public async Task<IEnumerable<IdentityUser>> GetUsersByCondition(Expression<Func<IdentityUser, bool>> expression)
        {
            return await context.Users.Where(expression).OrderBy(x => x.UserName).ToListAsync();
        }

        public async Task<IEnumerable<IdentityRole>> GetUserRolesByUserId(Guid id)
        {
            var roles = await (from role in context.Roles
                               join userRole in context.UserRoles
                               on role.Id equals userRole.RoleId
                               where userRole.UserId == id.ToString()
                               select role).ToListAsync();
            return roles;
        }

        public async Task AddRoleToUser(UserProfileEnum role, IdentityUser user)
        {
            await userManager.AddToRoleAsync(user, role.ToString());
        }
    }
}
