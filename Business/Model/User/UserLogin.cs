using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.User
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Veuillez entrer votre password")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Veuillez entrer votre email")]
        public string? Email { get; set; }
    }
}
