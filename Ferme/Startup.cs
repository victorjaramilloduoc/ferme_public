using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FermeBackend;
using Ferme.IdentityProvider;
using FermeMVCAuth.IdentityProvider;

namespace Ferme
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Hot reload
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddScoped<IPasswordHasher<FermeUser>, FermePasswordHasher>();
            //TODO: Falta un tokenProvider si quisiera implementar reset con tokens
            services.AddDefaultIdentity<FermeUser>()
                .AddUserStore<FermeUserStore>()
                .AddClaimsPrincipalFactory<FermeUserClaimsPrincipalFactory>()
                .AddErrorDescriber<SpanishIdentityErrorDescriber>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddHttpClient<api_docsClient>("FermeBackendClient");
            services.Configure<IdentityOptions>(options => 
            {
                // Restricciones de contraseña para usuarios
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //crear ruta para las areas creadas

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute("Users","Users", "{controller=Home}/{action=Index}/{id?}"); // Esta linea se debe eliminar
                endpoints.MapRazorPages();
            });
        }
    }
}
