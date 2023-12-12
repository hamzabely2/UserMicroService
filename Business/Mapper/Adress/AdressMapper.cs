using Model.Adress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper.Adress
{
    public class AdressMapper
    {
        public static AdressRead TransformDtoExit(Entity.Model.Adress address)
        {
            return new AdressRead()
            {
                City = address.City,
                State = address.State,
                Code = address.Code,
                Street = address.Street,
            };
        }

        public static Entity.Model.Adress TransformDtoAdd(AdressAdd request)
        {
            return new Entity.Model.Adress()
            {
                City = request.City,
                State = request.State,
                Code = request.Code,
                Street = request.Street,
                CreatedDate = DateTime.Now,
                UpdateDate= DateTime.Now,                           
            };
        }

        public static Entity.Model.Adress TransformDtoUpdate(AdressAdd request, Entity.Model.Adress uniteGet)
        {
            uniteGet.State = request.State;
            uniteGet.Street = request.Street;
            uniteGet.City = request.City;
            uniteGet.Code = request.Code;
            uniteGet.UpdateDate = DateTime.Now;

            return uniteGet;
        }
    }
}
