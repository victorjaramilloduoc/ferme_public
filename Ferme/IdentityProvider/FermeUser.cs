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
        public String Password { get; set; }
    }
}
