using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ferme.Models;
using FermeBackend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Ferme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // injección de dependencias
        private IHttpClientFactory _clientFactory;

        public ProductController(IHttpClientFactory clientFactory){
            _clientFactory = clientFactory;
        }

        [HttpGet]
        [Route("producto")]
        public async Task<List<ProductModel>> ObtenerProductos()
        {
            List<ProductModel> Productos = new List<ProductModel>();
            api_docsClient clienteAPI = new api_docsClient(_clientFactory.CreateClient("FermeBackendClient"));
            var productosAPI = ((JArray)await clienteAPI.GetProductsUsingGETAsync()).ToObject<List<ProductEntity>>();
            foreach (var productoAPI in productosAPI)
            {
                var producto = new ProductModel()
                {
                    CodigoProducto = productoAPI.ProductCode.Value,
                    Nombre = productoAPI.Name,
                    Descripcion = productoAPI.Description,
                    Stock = productoAPI.Stock.Value,
                    Precio = productoAPI.Price.Value,
                    MarcaProducto = productoAPI.MarcaProducto
                };
                Productos.Add(producto);
            }
            return Productos;
        }
    }
}