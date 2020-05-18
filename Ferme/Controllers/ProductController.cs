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
                    Id = productoAPI.Id.Value,
                    CodigoProducto = productoAPI.ProductCode.Value,
                    Nombre = productoAPI.Name,
                    Imagen = productoAPI.ProductImage,
                    Stock = productoAPI.Stock.Value,
                    Precio = productoAPI.Price.Value,
                    MarcaProducto = productoAPI.BrandProduct
                };
                Productos.Add(producto);
            }
            return Productos;
        }

        [HttpGet]
        [Route("producto/{id}")]
        public async Task<DetailProductModel> ObtenerProductoId(long id)
        {
            api_docsClient clienteAPI = new api_docsClient(_clientFactory.CreateClient("FermeBackendClient"));
            var productoAPI = ((JObject)await clienteAPI.SearchProductUsingGETAsync(id)).ToObject<ProductEntity>();
            var producto = new DetailProductModel()
            {
                Id = productoAPI.Id.Value,
                CodigoProducto = productoAPI.ProductCode.Value,
                Nombre = productoAPI.Name,
                Imagen = productoAPI.ProductImage,
                Descripcion = productoAPI.Description,
                Stock = productoAPI.Stock.Value,
                Precio = productoAPI.Price.Value,
                MarcaProducto = productoAPI.BrandProduct
            };
            return producto;
        }
    }
}