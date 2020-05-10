using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            // Encodeo de la clave ingresada con Base64/UTF-8
            String encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(providedPassword));
            return hashedPassword == encodedPassword ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
