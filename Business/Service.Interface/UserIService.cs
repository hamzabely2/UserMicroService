using Api.Business.UserMicroService.Model.User;
using Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface UserIService
    {
        Task<UserRead> GetUserByName(string name);
        Task<string> Register(UserRegister request);
        Task<string> Login(UserLogin request);
        Task<UserRead> Update(UserUpdate request);
        Task<UserRead> UpdatePassword(UserPassword request);
        Task<UserRead> Delete(int userId);
    }
}
