
using Context.Interface;
using Entity.Model;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RoleUserRepository : GenericRepository<RoleUser>, RoleUserIRepository
    {
        public RoleUserRepository(UserMicroServiceIDbContext idbcontext) : base(idbcontext)
        {
        }


    }
}
