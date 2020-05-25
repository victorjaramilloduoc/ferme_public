using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Ferme.Models;
using FermeBackend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;

namespace Ferme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase{
        private IHttpClientFactory _clientFactory;
        public SaleController(IHttpClientFactory clientFactory){
            _clientFactory = clientFactory;
        }
        [Authorize]
        [HttpPost]
        [Route("Sale")]
        public async Task<String> Sale(SaleModel sale){
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            api_docsClient clienteAPI = new api_docsClient(_clientFactory.CreateClient("FermeBackendClient"));
            ProductSaleEntity saleApi = new ProductSaleEntity();
            //saleApi.
            await clienteAPI.GetSalesUsingPOSTAsync(saleApi);
            return "OK";
        }
    }
}