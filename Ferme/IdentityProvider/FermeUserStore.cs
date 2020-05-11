using FermeBackend;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ferme.IdentityProvider
{
    public class FermeUserStore : IUserStore<FermeUser>, IUserLoginStore<FermeUser>, IUserPasswordStore<FermeUser>, IUserEmailStore<FermeUser>
    {
        // Se trae el factory, para pedir que crear clientes para el backend
        private IHttpClientFactory _clientFactory;

        public FermeUserStore(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public Task AddLoginAsync(FermeUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Realiza un POST en base a los datos ingresados en el formulario de registro
        public async Task<IdentityResult> CreateAsync(FermeUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            //TODO: Registro debe contener mas datos
            UserEntity usuario = new UserEntity()
            {
                Id = 0,
                Name = user.FirstName,
                BirthDate = user.BirthDate,
                Rut = user.Rut,
                Dv = user.Dv,
                Enable = true,
                Genere = user.Genere,
                LastName = user.LastName + " " + user.SecondSurname,
                Email = user.Email,
                Password = user.PasswordHash,
                //Phone=user.Phone
                Address = user.Address + " " + user.Block,
                Location = user.Location
            };

            // Llama  al clientFactory y pide un cliente para el backend
            api_docsClient clienteAPI = new api_docsClient(_clientFactory.CreateClient("FermeBackendClient"));
            JObject response = (JObject) await clienteAPI.SaveUserUsingPOSTAsync(usuario);
            if (response["status"].Value<string>() == "error")
            {
                var error = new IdentityError()
                {
                    Code = "API",
                    Description = "Error guardando el usuario nuevo"
                };
                return IdentityResult.Failed(error);
            }
            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(FermeUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task<FermeUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<FermeUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<FermeUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Busca a un usuario por su nombre de usuario (Normalizado con mayúscula)
        public async Task<FermeUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            PasswordHasher<FermeUser> passwordHasher = new PasswordHasher<FermeUser>();
            api_docsClient clienteAPI = new api_docsClient(_clientFactory.CreateClient("FermeBackendClient"));
            JArray usuarios = (JArray)await clienteAPI.GetUsersUsingGETAsync();
            foreach (var usuario in usuarios)
            {
                var usuarioApi = usuario.ToObject<UserEntity>();
                if (usuarioApi.Email.ToUpper() == normalizedUserName)
                {
                    return new FermeUser()
                    {
                        SecurityStamp = Guid.NewGuid().ToString(),
                        PasswordHash =  usuarioApi.Password,
                        Email = usuarioApi.Email,
                        Login = usuarioApi.Email,
                        NormalizedUserName = usuarioApi.Email.ToUpper(),
                        NormalizedEmail = usuarioApi.Email.ToUpper(),
                        Id = usuarioApi.Id.GetValueOrDefault(),
                        UserName = usuarioApi.Email,
                        FirstName = usuarioApi.Name,
                        LastName = usuarioApi.LastName
                    };
                }
            }
            return null;
        }

        // Saca el campo de email
        public Task<string> GetEmailAsync(FermeUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult<string>(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(FermeUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(FermeUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedEmailAsync(FermeUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(FermeUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Llama al password hasheado (almacena una clave imposible de decifrar)
        public Task<string> GetPasswordHashAsync(FermeUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetUserIdAsync(FermeUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(FermeUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(FermeUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(FermeUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // toma el mail que viene del formulario y lo coloca al usuario del front
        public Task SetEmailAsync(FermeUser user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.Email = email;
            return Task.FromResult<object>(null);
        }

        public Task SetEmailConfirmedAsync(FermeUser user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(FermeUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult<object>(null);
        }

        // Se trae el nombre del usuario del formulario (normalizado) y lo copia al usuario del front end
        public Task SetNormalizedUserNameAsync(FermeUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(user.NormalizedUserName);
        }

        // Se trae el password del usuario del formulario (normalizado) y lo copia al usuario del front end
        public Task SetPasswordHashAsync(FermeUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.PasswordHash = passwordHash;
            return Task.FromResult(user.PasswordHash);
        }

        public Task SetUserNameAsync(FermeUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.UserName = userName;
            user.Login = userName;
            return Task.FromResult<object>(null);
        }

        public Task<IdentityResult> UpdateAsync(FermeUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
