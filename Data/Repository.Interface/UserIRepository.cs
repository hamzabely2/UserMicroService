using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface UserIRepository : GenericIRepository<User>
    {
        Task<User> GetUserByName(string name);
        Task<User> GetUserByEmail(string email);

    }
}
