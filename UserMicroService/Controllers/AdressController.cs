
using Entity.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Adress;
using Service.Interface;

namespace UserMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RoleString.User)]
    public class AdressController : Controller
    {
        private readonly AdressIService _adressService;
        public AdressController(AdressIService adressService)
        {
            _adressService = adressService;
        }

        /// add addresse <summary>
        /// </summary>
        /// <param name="commentsDto"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(AdressRead), 200)]
        [ProducesResponseType(typeof(StatusCodeResult), 500)]
        [ProducesResponseType(typeof(StatusCodeResult), 400)]
        public async Task<ActionResult<AdressRead>> FuncAdd(AdressAdd request)
        {
            try
            {
                AdressRead result = await _adressService.Add(request);
                string message = "l'adresse a été ajoutée avec succès";
                return Ok(new { message, result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        ///  get addresse for user <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet("user")]
        [ProducesResponseType(typeof(AdressRead), 200)]
        [ProducesResponseType(typeof(StatusCodeResult), 500)]
        [ProducesResponseType(typeof(StatusCodeResult), 400)]
        public async Task<ActionResult> FuncAdd()
        {
            try
            {
                List<AdressRead> result = await _adressService.GetAddresseForUser();
                string message = "les adresses de cet utilisateur";
                return Ok(new { message, result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        /// update addresse <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="adressId"></param>
        /// <returns></returns>
        [HttpPut("update/{adressId}")]
        [ProducesResponseType(typeof(AdressRead), 200)]
        [ProducesResponseType(typeof(StatusCodeResult), 500)]
        [ProducesResponseType(typeof(StatusCodeResult), 400)]
        public async Task<ActionResult> FuncUpdate(AdressAdd request, int adressId)
        {
            try
            {
                AdressRead result = await _adressService.Update(request, adressId);
                string message = "la modification  de l'adresse a réussi";
                return Ok(new { message, result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// delete comment <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(AdressRead), 200)]
        [ProducesResponseType(typeof(StatusCodeResult), 500)]
        [ProducesResponseType(typeof(StatusCodeResult), 400)]
        public async Task<ActionResult> FuncDelete(int id)
        {
            try
            {
                var result = await _adressService.Delete(id);
                string message = "la suppression de l'adresse a réussi";
                return Ok(new { message, result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
