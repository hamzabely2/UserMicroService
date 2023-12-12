
using Api.Business.UserMicroService.Model.User;
using Entity.Model;
using Mapper.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model.User;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserService : UserIService
    {

        private readonly UserIRepository _userRepository;
        private readonly RoleIService _roleService;
        private readonly RoleIRepository _roleRepository;
        private readonly IConfiguration _configuration;
        private readonly ConnectionIService _connectionService;

        public UserService(UserIRepository userRepository, IConfiguration configuration, RoleIService roleService, RoleIRepository roleRepository, ConnectionIService connectionService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _roleService = roleService;
            _roleRepository = roleRepository;
            _connectionService = connectionService;


        }

        /// get user by name <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<UserRead> GetUserByName(string name)
        {
            User user = await _userRepository.GetUserByName(name).ConfigureAwait(false);
            if (user == null)
                throw new ArgumentException($"le user {name} ne existe pas.");

            return UserMapper.TransformDtoExit(user);
        }

        public async Task<string> Register(UserRegister request)
        {
            _roleService.AddRoles();

            var firstNameExiste = _userRepository.GetUserByName(request.FirstName);
            if (firstNameExiste.Result != null)
                throw new ArgumentException("l'action a échoué : le nom existe déjà");

            var emailNameExiste = _userRepository.GetUserByEmail(request.Email);
            if (emailNameExiste.Result != null)
                throw new ArgumentException("l'action a échoué :l'email existe déjà");

            if (!_connectionService.IsValidEmail(request.Email))
                throw new ArgumentException("Adresse e-mail non valide");

            if(!_connectionService.IsPasswordValid(request.Password))
                throw new ArgumentException("les mots de passe doivent comporter au moins un chiffre ('0' - '9'). Les mots de passe doivent contenir au moins une majuscule ('A' - 'Z').");

            var passwordHash = _connectionService.HashPassword(request.Password);
            var newUser = UserMapper.TransformDtoRegister(request, passwordHash);

            //create user
            var user = await _userRepository.CreateElementAsync(newUser).ConfigureAwait(false);
            if (user == null)
                throw new ArgumentException("l'enregistrement n'a pas réussi, quelque chose s'est mal passé");


            //addin role of a user
            var roleAssignmentResult = await _roleService.AssignRoleAsync(user.Id, 1);
            if (roleAssignmentResult == null)
            {
                throw new ArgumentException("L'enregistrement n'a pas réussi, quelque chose s'est mal passé lors de l'attribution du rôle");
            }

            //adding info user of a token 
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            };
            foreach (var roleUser in user.Roles_Users)
            {
                var role = await _roleRepository.GetRoleOfAUser(roleUser).ConfigureAwait(false);
                if (role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }
            }

            //create token
            var token = _connectionService.CreateToken(claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        /// Login <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<string> Login(UserLogin request)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user == null || !_connectionService.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new ArgumentException($"La connexion a échoué : e-mail ou mot de passe incorrect.");
            }

            
            var role = _roleRepository.GetRole(user.Id); ;
            var claims = new List<Claim>
                {
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role.Result[0].Name),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
           
            
            var token = _connectionService.CreateToken(claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        /// Update user <summary>
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<UserRead> Update(UserUpdate request)
        {
            var userInfo = _connectionService.GetCurrentUserInfo();
            int userConnectedId = userInfo.Id;
            var userToUpdate = await _userRepository.GetByKeys(userConnectedId);
            if (userToUpdate == null)
                throw new ArgumentException("l'action a échoué : l'ulisateur n'a pas été trouvée");

            var userExistsUserName = await _userRepository.GetUserByName(request.FirstName);
            if (userExistsUserName != null)
                throw new ArgumentException("l'action a échoué :le nom existe déjà");

            var userExistsEmail = await _userRepository.GetUserByEmail(request.Email);
            if (userExistsEmail != null)
                throw new ArgumentException("l'action a échoué :l'email existe déjà");

            var user = UserMapper.TransformDtoUpdate(request, userToUpdate);

            var userUpdate = await _userRepository.UpdateElementAsync(user).ConfigureAwait(false);
            if (userUpdate == null)
                throw new ArgumentException("l'action a échoué : la modification des données utilisateur a échoué");
            var resultUserUpdate = UserMapper.TransformDtoExit(userUpdate);

            return resultUserUpdate;
        }

        /// Update password to user <summary>
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<UserRead> UpdatePassword(UserPassword request)
        {
            var userInfo = _connectionService.GetCurrentUserInfo();
            int userConnectedId = userInfo.Id;
            var user = await _userRepository.GetByKeys(userConnectedId);

            if (!_connectionService.VerifyPassword(request.OldPassword, user.PasswordHash))
            {
                throw new ArgumentException($"La connexion a échoué : l'ancien mot de passe n'est pas correct");
            }        

            if (request.ConfirmNewPassword != request.NewPassword)
                throw new ArgumentException("l'action a échoué: le nouveau mot de passe ne correspond pas au mot de passe de confirmation");


            // Mettre à jour le mot de passe dans la base de données
            if (!_connectionService.IsPasswordValid(request.NewPassword))
                throw new ArgumentException("l'action a échoué: les mots de passe doivent comporter au moins un chiffre ('0' - '9'). Les mots de passe doivent contenir au moins une majuscule ('A' - 'Z').\\\"\"");

            if (request.OldPassword == request.NewPassword)
                throw new ArgumentException("l'action a échoué: le nouveau mot de passe est le même que l'ancien");

            user.PasswordHash = request.NewPassword;
            await _userRepository.UpdateElementAsync(user);
            var changePasswordResult = UserMapper.TransformDtoExit(user);

            return changePasswordResult;
        }


        /// Delete user <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<UserRead> Delete(int userId)
        {
            var user = await _userRepository.GetByKeys(userId);

            if (user == null)
                throw new ArgumentException("l'action a échoué : l'ulisateur n'a pas été trouvée");

            await _roleService.DeleteRoleAsync(userId);
            User userDelete = await _userRepository.DeleteElementAsync(user);
            if (userDelete == null)
                throw new ArgumentException($"L'utilisateur ne existe pas.");

            return UserMapper.TransformDtoExit(userDelete);
        }
    }
}
