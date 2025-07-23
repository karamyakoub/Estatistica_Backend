using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.ReporsitoryContracts
{
    public interface IUsuarioRepository
    {
        Task<IdentityUser?> GetUsuarioById(string id);
        Task<bool> ChangeUserStatus(string userId, bool isActive);
        Task<IdentityUser> AddUser(IdentityUser user, string password);
        Task<IdentityUser?> GetUsuarioByUserName(string userName);
        Task<IEnumerable<IdentityUser>> GetUsers();
        Task<IEnumerable<IdentityUser>> GetUsersByCondition(Expression<Func<IdentityUser, bool>> expression);
        Task<IEnumerable<IdentityRole>> GetUserRolesByUserId(Guid id);
        Task AddRoleToUser(UserProfileEnum role, IdentityUser user);

    }
}
