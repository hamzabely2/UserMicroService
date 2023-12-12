using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface RoleIService
    {
        void AddRoles();
        Task<RoleUser> AssignRoleAsync(int userId, int roleId);
        Task DeleteRoleAsync(int userId);

    }
}
