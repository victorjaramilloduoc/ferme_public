using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferme.IdentityProvider
{
    /// <summary>
    /// Clase base de usuario para core.Identity
    /// </summary>
    public class FermeUser : IdentityUser<long>
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SecondSurname { get; set; }


    }
}
