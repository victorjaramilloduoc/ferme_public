using FermeBackend;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ferme.Areas.Identity.Pages.Products
{
    public class ProductsStore
    {
        private IHttpClientFactory _clientFactory;

        public ProductsStore(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }



        public async Task<ListProductsModel> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            api_docsClient clienteAPI = new api_docsClient(_clientFactory.CreateClient("FermeBackendClient"));
            JArray productos = (JArray)await clienteAPI.GetProductsUsingGETAsync();
            foreach (var producto in productos)
            {
                var usuarioApi = producto.ToObject<ProductEntity>();
                return new ListProductsModel()
                {
                    Id = usuarioApi.Id.GetValueOrDefault(),
                     Description=usuarioApi.Description,
                     Name=usuarioApi.Name,
                      Price=usuarioApi.Price,
                       Stock=usuarioApi.Stock,
                        ProductCode=usuarioApi.ProductCode,
                         SupplierId=usuarioApi.SupplierId
                    };
                
            }
            return null;
        }
    }
    }
