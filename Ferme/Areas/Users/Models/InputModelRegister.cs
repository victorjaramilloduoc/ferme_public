using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferme.Areas.Users.Models
{
    public class InputModelRegister
    {
        //le decimos al framework un msj en caso de error y creamos validaciones inmediatamente
        [Required(ErrorMessage = "El campo nombre es obligatorio.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "El campo apellido es obligatorio.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El campo Rut es obligatorio.")]
        public string Rut { get; set; }

        [Required(ErrorMessage ="El campo Email es obligatorio")]
        [EmailAddress(ErrorMessage ="El campo email no es una direccion valida")]
        public string Email { get; set; }

        [Display(Name ="Contraseña")]
        [Required(ErrorMessage ="El campo contraseña es obligatorio")]
        [StringLength(100, ErrorMessage ="el numero de caracteres de {0} de ser al menos {2}", MinimumLength =6)]
        public string Password { get; set; }
    }
}
