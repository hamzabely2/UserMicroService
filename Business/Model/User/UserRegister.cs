using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.User
{
    public class UserRegister
    {
        [Required(ErrorMessage = "Veuillez entrer votre nom")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre prenom")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre password")]
        public string? Password { get; set; }
    }
}
