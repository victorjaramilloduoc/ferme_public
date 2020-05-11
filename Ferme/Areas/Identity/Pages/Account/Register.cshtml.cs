using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Ferme.IdentityProvider;
using FermeBackend;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Ferme.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<FermeUser> _signInManager;
        private readonly UserManager<FermeUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IHttpClientFactory _clientFactory;

        public RegisterModel(
            UserManager<FermeUser> userManager,
            SignInManager<FermeUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IHttpClientFactory httpClientFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _clientFactory = httpClientFactory;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public List<SelectListItem> Regiones { get; set;}
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Debes ingresar un correo eléctronico")]
            [EmailAddress(ErrorMessage = "Debes ingresar un correo válido")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Debes ingresar una contraseña")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "la contraseña y su confirmación no son iguales")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Debes ingresar un Nombre")]
            [Display(Name = "Nombre")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Debes ingresar un Apellido Paterno")]        
            [Display(Name = "Apellido Paterno")]
            public string LastName { get; set; }

            [Display(Name = "Apellido Materno")]
            public string SecondSurname { get; set; }

            [Required(ErrorMessage = "Debes ingresar cédula de identidad")]
            [StringLength(8, ErrorMessage = "Existe un error en la cantidad de caracteres", MinimumLength = 7)]
            [Display(Name = "Rut")]
            public string Rut { get; set; }

            [StringLength(1)]
            public string Dv { get; set; }

            public string Genere { get; set; }
            public int Phone { get; set; }
            public string Address { get; set; }
            public string Block { get; set; }
            public System.DateTimeOffset? BirthDate { get; set; }
            public String Location { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //Regiones
            api_docsClient clientAPI = new api_docsClient(_clientFactory.CreateClient("FermeBackendClient"));
            var regiones = ((JArray) await clientAPI.GetRegionUsingGETAsync()).ToObject<List<RegionEntity>>();
            Regiones = new List<SelectListItem>();
            foreach (var region in regiones)
            {
                Regiones.Add(new SelectListItem()
                {
                    Text = region.RegionName,
                    Value = region.Id.ToString()
                });
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new FermeUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    SecondSurname = Input.SecondSurname,
                    Rut= int.Parse(Input.Rut),
                    Dv=Input.Dv,
                    Genere=Input.Genere,
                    //Phone=Input.Phone
                    Address=Input.Address,
                    BirthDate=Input.BirthDate,
                    Block=Input.Block
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
