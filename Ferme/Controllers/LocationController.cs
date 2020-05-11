using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FermeBackend;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;

namespace Ferme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private IHttpClientFactory _clientFactory;
        public LocationController(IHttpClientFactory clientFactory){
            _clientFactory = clientFactory;
        }

        [HttpGet]
        [Route("region/{idRegion}/ciudades")]
        public async Task<List<SelectListItem>> ObtenerCiudadesPorRegion(int idRegion)
        {
            List<SelectListItem> listaCiudades = new List<SelectListItem>();
            api_docsClient clienteAPI = new api_docsClient(_clientFactory.CreateClient("FermeBackendClient"));
            var ciudades = ((JArray)await clienteAPI.GetCitiesUsingGETAsync()).ToObject<List<CityEntity>>();
            var ciudadesRegion = from ciudad in ciudades where ciudad.Region.Id == idRegion select ciudad;
            foreach (var ciudad in ciudadesRegion)
            {
                SelectListItem itemCiudad = new SelectListItem();
                itemCiudad.Text = ciudad.CityName;
                itemCiudad.Value = ciudad.Id.ToString();
                listaCiudades.Add(itemCiudad);
            }
            return listaCiudades;
        }

        [HttpGet]
        [Route("ciudad/{idCiudad}/comunas")]
        public async Task<List<SelectListItem>> ObtenerComunasPorCiudad(int idCiudad)
        {
            List<SelectListItem> listaComunas = new List<SelectListItem>();
            api_docsClient clienteAPI = new api_docsClient(_clientFactory.CreateClient("FermeBackendClient"));
            var comunas = ((JArray)await clienteAPI.GetLocationsUsingGETAsync()).ToObject<List<LocationEntity>>();
            var comunasCiudad = from comuna in comunas where comuna.City.Id == idCiudad select comuna;
            foreach (var comuna in comunasCiudad)
            {
                SelectListItem itemComuna = new SelectListItem();
                itemComuna.Text = comuna.LocatioName;
                itemComuna.Value = comuna.Id.ToString();
                listaComunas.Add(itemComuna);
            }
            return listaComunas;
        }
    }
}