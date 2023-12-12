using Api.Business.UserMicroService.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.User;
 

namespace Mapper.User
{
    public  class UserMapper
    {
        public static UserRead TransformDtoExit(Entity.Model.User user)
        {
            return new UserRead()
            {
                Email = user.Email,
                FirstName = user.FirstName,
            };
        }

        public static Entity.Model.User TransformDtoRegister(UserRegister request,string passwordHash)
        {
            return new Entity.Model.User()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash= passwordHash,
                CreatedDate = DateTime.Now,
                UpdateDate= DateTime.Now,
            };
        }

        public static Entity.Model.User TransformDtoUpdate(UserUpdate request, Entity.Model.User uniteGet)
        {
            uniteGet.Email = request.Email;
            uniteGet.FirstName = request.FirstName;
            uniteGet.LastName = request.LastName;
            uniteGet.PhoneNumber = request.PhoneNumber;
            uniteGet.UpdateDate = DateTime.Now;

            return uniteGet;
        }
    }
}
