using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ferme.IdentityProvider
{
	/// <summary>
	/// Esta clase añade claims (información adicional acerca de la identidad del usuario) a partir de la información del login.
	/// </summary>
	public class FermeUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<FermeUser>
	{
		public FermeUserClaimsPrincipalFactory(UserManager<FermeUser> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
		{
		}

		/// <summary>
		/// Método de añadir claims.
		/// Se añaden adicionalmente los siguientes:
		/// DisplayName: Nombre a mostrar en el header
		/// ApiToken: Token proveniente del método Login en el API Ferme para acceder a sus métodos
		/// </summary>
		/// <param name="user">Usuario proveniente del API</param>
		/// <returns>Conjunto de Claims</returns>
		protected override async Task<ClaimsIdentity> GenerateClaimsAsync(FermeUser user)
		{
			var identity = await base.GenerateClaimsAsync(user);
			identity.AddClaim(new Claim("DisplayName", user.FirstName ?? "Anónimo"));
			identity.AddClaim(new Claim("ApiToken", user.Token ?? ""));
			return identity;
		}
	}
}
