
using Context.Interface;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AdressRepository : GenericRepository<Adress>, AdressIRepository
    {
        public AdressRepository(UserMicroServiceIDbContext _idbcontext) : base(_idbcontext)
        {
            _table = _idbcontext.Set<Adress>();
        }
        private readonly DbSet<Adress> _table;


        //add adresse to user
        public async Task<bool> AddAdressToUser(int userId, int adressId)
        {
            var user = await _idbcontext.Users.FindAsync(userId);
            var adresse = await _table.FindAsync(adressId);

            if (user != null && adresse != null)
            {
                var addressUser = new AdressUser
                {
                    UserId = userId,
                    AdressId = adressId
                };

                _idbcontext.AdressUsers.Add(addressUser);
                await _idbcontext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        /// <summary>
        /// get addresses for user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Adress>> GetAdressesForUser(int userId)
        {
            var adresseIds = await _idbcontext.AdressUsers
                .Where(au => au.UserId == userId)
                .Select(au => au.AdressId)
                .ToListAsync();

            var adresses = await _idbcontext.Adresses
                .Where(a => adresseIds.Contains(a.Id))
                .ToListAsync();

            return adresses;
        }
    }
}
