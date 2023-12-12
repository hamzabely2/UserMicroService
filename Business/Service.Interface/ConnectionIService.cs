using Entity.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ConnectionIService
    {
        bool IsValidEmail(string email);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
        JwtSecurityToken CreateToken(IEnumerable<Claim> claims);
        List<Claim> CreateClaims(User user);
        bool IsPasswordValid(string password);
        UserInfo GetCurrentUserInfo();
    }
}
