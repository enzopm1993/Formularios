using Asiservy.Automatizacion.Formularios.AccesoDatos.BLZ;
using Asiservy.Automatizacion.Formularios.Models;
using Asiservy.Automatizacion.Formularios.Models.Produccion;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.General
{
    public class clsDApiProduccion
    {
        public object ConsultarObservaciones(string Codigo)
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            RestRequest request;
            if (string.IsNullOrEmpty(Codigo))
                request = new RestRequest("/api/Produccion/Observaciones", Method.GET);
            else
                request = new RestRequest("/api/Produccion/Observaciones/" + Codigo + "/", Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var content = response.Content;
            var ListaTallas = JsonConvert.DeserializeObject(content);
            return ListaTallas;

        }

        public List<Textura> ConsultarObservaciones()
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            RestRequest request;

            request = new RestRequest("/api/Produccion/Observaciones", Method.GET);

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var content = response.Content;
            var ListaTallas = JsonConvert.DeserializeObject<List<Textura>>(content);
            return ListaTallas;

        }

        public object ConsultarTallas(string Talla)
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            RestRequest request;
            if (string.IsNullOrEmpty(Talla))
                request = new RestRequest("/api/Produccion/Tallas", Method.GET);
            else
                request = new RestRequest("/api/Produccion/Tallas/" + Talla + "/", Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var content = response.Content;
            var ListaTallas = JsonConvert.DeserializeObject(content);
            return ListaTallas;

        }

        public object ConsultarLotesPorFecha(DateTime Fecha)
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            RestRequest request;
            request = new RestRequest("/api/Produccion/LotesPorFecha/" + Fecha.Year + "-" + Fecha.Month + "-" + Fecha.Day + "/", Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var content = response.Content;
            var ListaLotes = JsonConvert.DeserializeObject(content);
            return ListaLotes;

        }
        public object ConsultarEspecies()
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            RestRequest request;
            request = new RestRequest("/api/Produccion/Especies", Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var content = response.Content;
            var ListaEspecies = JsonConvert.DeserializeObject(content);
            return ListaEspecies;
        }


        public object ConsultarBarcos()
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            RestRequest request;
            request = new RestRequest("/api/Produccion/Barcos", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var Lista = JsonConvert.DeserializeObject(content);
            return Lista;
        }

        public List<InsumosProduccion> ConsultaAditivos()
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            RestRequest request;
            request = new RestRequest("/api/Produccion/InsumosProduccion", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var Lista = JsonConvert.DeserializeObject<List<InsumosProduccion>>(content);
            return Lista;
        }


        public List<RegistroDescongeladoEmparrilladoMP> ConsultaControlDescongeladoEmparrilladoMP(DateTime fechaPrd)
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);

            string URL = "/api/Produccion/ControlDescongeladoEmparrilladoMP/" + fechaPrd.ToString("yyyy-MM-dd");

            var request = new RestRequest(URL, Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var datos = JsonConvert.DeserializeObject<List<RegistroDescongeladoEmparrilladoMP>>(content);
            return datos;
         }


        public List<Rendimiento> ConsultaRendimientos()
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            RestRequest request;
            request = new RestRequest("/api/Produccion/Rendimientos", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            List<Rendimiento> Lista=null;
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return Lista;
            }
            Lista = JsonConvert.DeserializeObject<List<Rendimiento>>(content);
            return Lista;
        }

    }
}