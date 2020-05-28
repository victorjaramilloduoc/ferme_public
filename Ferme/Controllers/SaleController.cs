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
using Newtonsoft.Json;
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
            try{
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                api_docsClient clienteAPI = new api_docsClient(_clientFactory.CreateClient("FermeBackendClient"));
                List<ProductSaleEntity> productsSalesApi = new List<ProductSaleEntity>();
                foreach (var product in sale.Cart){
                    ProductSaleEntity productSaleAPI = new ProductSaleEntity();
                    productSaleAPI.Product = new ProductEntity{
                        Id = product.Id
                    };
                    productSaleAPI.Quantity = 1;
                    productsSalesApi.Add(productSaleAPI);
                }
                SaleEntity saleApi = new SaleEntity
                {
                    Products_sale = productsSalesApi,
                    User = new UserEntity
                    {
                        Id=long.Parse(identity.FindFirst("Id").Value)
                    },
                    Document = new DocumentEntity
                    {
                        Document_type = new DocumentTypeEntity
                        {
                            Id = int.Parse(sale.DocumentType)
                        }
                    },
                    Payment_method = new PaymentMethodEntity
                    {
                        Id=1
                    }
                };
                Console.Out.WriteLine(JsonConvert.SerializeObject(saleApi));
                await clienteAPI.RecordedSaleUsingPOSTAsync(saleApi);
            }
            catch(Exception ex){
                Console.Out.WriteLine(ex.Message);
                throw (ex);
            }
            
            return "OK";

        }
    }
}