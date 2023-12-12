using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface RoleIRepository : GenericIRepository<Role>
    {
        Task<Role> GetRoleOfAUser(RoleUser roleUser);

        Task<List<Role>> GetRole(int userId);
    }
}
