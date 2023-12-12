using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Adress
{
    public class AdressAdd
    {

        [Required(ErrorMessage = "insérez la Ville")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "insérez l'États")]
        public string State { get; set; } = null!;

        [Required(ErrorMessage = "insérez la Rue")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "insérez un code postal")]
        public int? Code { get; set; }
    }
}
