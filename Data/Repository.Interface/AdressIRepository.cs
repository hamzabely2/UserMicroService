using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface AdressIRepository : GenericIRepository<Adress>
    {
        Task<bool> AddAdressToUser(int userId, int adressId);
        Task<List<Adress>> GetAdressesForUser(int userId);
    }
}
