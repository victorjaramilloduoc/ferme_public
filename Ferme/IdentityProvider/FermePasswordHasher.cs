using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferme.IdentityProvider
{
    /// <summary>
    /// Esta clase deshabilita el hashing para enviar la clave como texto plano al crear usuario
    /// </summary>
    public class FermePasswordHasher : IPasswordHasher<FermeUser>
    {
        public string HashPassword(FermeUser user, string password)
        {
            return password;
        }

        public PasswordVerificationResult VerifyHashedPassword(FermeUser user, string hashedPassword, string providedPassword)
        {
            return hashedPassword == providedPassword ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
