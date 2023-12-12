using Api.Business.UserMicroService.Model.User;
using Entity.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.User;
using Service.Interface;

namespace UserMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserIService _userService;
        public UserController(UserIService userService)
        {
            _userService = userService;
        }

        /// get user by name <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [Authorize(Roles = RoleString.Admin)]
        [ProducesResponseType(typeof(UserRead), 200)]
        [ProducesResponseType(typeof(StatusCodeResult), 500)]
        [ProducesResponseType(typeof(StatusCodeResult), 400)]
        public async Task<ActionResult> FuncGetUser(string name)
        {
            try
            {
                var result =  await _userService.GetUserByName(name).ConfigureAwait(false);
                string message = "user";
                return Ok(new { message, result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// register user <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(StatusCodeResult), 200)]
        [ProducesResponseType(typeof(StatusCodeResult), 500)]
        [ProducesResponseType(typeof(StatusCodeResult), 400)]
        public async Task<IActionResult> FuncRegister(UserRegister request)
        {
            try
            {
                var result = await _userService.Register(request);
                string message = "inscription réussie";
                return Ok(new { message, result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                return BadRequest(new { message = ex.Message });
            }
        }

        ///login user <summary>
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserRead), 200)]
        [ProducesResponseType(typeof(StatusCodeResult), 500)]
        [ProducesResponseType(typeof(StatusCodeResult), 400)]
        public async Task<ActionResult> FuncLogin(UserLogin request)
        {
            try
            {
                var result = await _userService.Login(request).ConfigureAwait(false);
                string message = "connexion réussie";
                return Ok(new { message, result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        /// update user <summary>
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Authorize(Roles = RoleString.User)]
        [ProducesResponseType(typeof(UserRead), 200)]
        [ProducesResponseType(typeof(StatusCodeResult), 500)]
        [ProducesResponseType(typeof(StatusCodeResult), 400)]
        public async Task<ActionResult> FuncUpdate(UserUpdate request)
        {
            try
            {
                var result = await _userService.Update(request);
                string message = "utilisateur modifié avec succès";
                return Ok(new { message, result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        /// update password to user <summary>
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("update/password")]
        [Authorize(Roles = RoleString.User)]
        [ProducesResponseType(typeof(UserRead), 200)]
        [ProducesResponseType(typeof(StatusCodeResult), 500)]
        [ProducesResponseType(typeof(StatusCodeResult), 400)]
        public async Task<ActionResult> FuncUpdate(UserPassword request)
        {
            try
            {
                var result = await _userService.UpdatePassword(request);
                string message = "Le mot de passe a été changé avec succès";
                return Ok(new { message, result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// delete user <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{userId}")]
        [Authorize(Roles = RoleString.Admin)]
        [Authorize(Roles = RoleString.User)]
        [ProducesResponseType(typeof(UserRead), 200)]
        [ProducesResponseType(typeof(StatusCodeResult), 500)]
        [ProducesResponseType(typeof(StatusCodeResult), 400)]
        public async Task<ActionResult> FuncDelete(int userId)
        {
            try
            {
                var result = await _userService.Delete(userId);
                string message = "utilisateur supprimé avec succès";
                return Ok(new { message, result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
    }
}
