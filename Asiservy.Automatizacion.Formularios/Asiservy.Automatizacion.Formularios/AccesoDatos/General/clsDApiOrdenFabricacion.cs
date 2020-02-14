using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Asiservy.Automatizacion.Formularios.Models;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.General
{
    public class clsDApiOrdenFabricacion
    {

        public object ConsultaLotesPorOrdenFabricacionLinea(int OrdernFabricacion, string Linea)
        {
            var client = new RestClient("http://192.168.0.31:8870");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("/api/Produccion/LotesPorOrdenLinea/" + OrdernFabricacion + "/" + Linea, Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var content = response.Content;
            var ListaUsuarios = JsonConvert.DeserializeObject(content);
            return ListaUsuarios;
        }

        public object ConsultaLotesPorOrdenFabricacion(int OrdenFabricacion)
        {
            var client = new RestClient("http://192.168.0.31:8003");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("/api/Produccion/LotesPorOrden/" + OrdenFabricacion, Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var content = response.Content;
            var ListaUsuarios = JsonConvert.DeserializeObject(content);
            return ListaUsuarios;
        }

        /// <summary>
        /// DEVUELVE LOS DATOS DE UN LOTE 
        /// </summary>
        /// <param name="OrdenFabricacion"></param>
        /// <returns></returns>
        public List<OfLote> ConsultaLotesPorOF(int OrdenFabricacion)
        {
            var client = new RestClient("http://192.168.0.31:8003");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("/api/Produccion/LotesPorOrden/" + OrdenFabricacion, Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var content = response.Content;
            var ListaUsuarios = JsonConvert.DeserializeObject<List<OfLote>>(content);
            return ListaUsuarios;
        }

        public List<OrdenFabricacionAvance> ConsultaLotesPorOrdenFabricacionLinea2(int OrdernFabricacion, string Linea)
        {
            var client = new RestClient("http://192.168.0.31:8870");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("/api/Produccion/LotesPorOrdenLinea/" + OrdernFabricacion + "/" + Linea, Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var content = response.Content;
            var ListaUsuarios = JsonConvert.DeserializeObject<List<OrdenFabricacionAvance>>(content);
            return ListaUsuarios;
        }


        public object ConsultaOrdenFabricacionPorFechaProduccion(DateTime FechaProduccion)
        {
            var client = new RestClient("http://192.168.0.31:8870");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("/api/Produccion/OrdenesFabricacionPorFecha/" + FechaProduccion.ToString("yyyy-MM-dd"), Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var content = response.Content;
            var ListaOrdenes = JsonConvert.DeserializeObject(content);
            return ListaOrdenes;

        }


        public object ConsultaOFNivel3(DateTime FechaProduccion)
        {
            var client = new RestClient("http://192.168.0.31:8003");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("/api/Produccion/OrdenesFabricacionNivel3/" + FechaProduccion.ToString("yyyy-MM-dd"), Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var content = response.Content;
            var ListaOrdenes = JsonConvert.DeserializeObject(content);
            return ListaOrdenes;

        }

        public List<OrdenFabricacionAutoclave> ConsultaOrdenFabricacionPorFechaProductoTerminado(DateTime FechaProduccion)
        {
            var client = new RestClient("http://192.168.0.31:8870");
            var request = new RestRequest("/api/Produccion/OrdenesAmbosNiveles/" + FechaProduccion.ToString("yyyy-MM-dd"), Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var content = response.Content;
            List<OrdenFabricacionAutoclave> ListaOrdenes = JsonConvert.DeserializeObject<List<OrdenFabricacionAutoclave>>(content);
            return ListaOrdenes;
        }



        public List<OrdenFabricacionAutoclave> ConsultaOrdenFabricacionPorFechaAutoclave(DateTime FechaProduccion)
        {
            var client = new RestClient("http://192.168.0.31:8870");
            var request = new RestRequest("/api/Produccion/OrdenesAutoclave/" + FechaProduccion.ToString("yyyy-MM-dd"), Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var content = response.Content;
            List<OrdenFabricacionAutoclave> ListaOrdenes = JsonConvert.DeserializeObject<List<OrdenFabricacionAutoclave>>(content);
            return ListaOrdenes;
        }

        public List<OrdenFabricacionConsumoInsumo> ConsultaOrdenFabricacionPorFechaConsumoInsumo(string Orden)
        {
            var client = new RestClient("http://192.168.0.31:8870");
            var request = new RestRequest("/api/Produccion/DatosOrdenFabricacion/" + Orden, Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var content = response.Content;
            List<OrdenFabricacionConsumoInsumo> ListaOrdenes = JsonConvert.DeserializeObject<List<OrdenFabricacionConsumoInsumo>>(content);
            return ListaOrdenes;
        }


    }
}