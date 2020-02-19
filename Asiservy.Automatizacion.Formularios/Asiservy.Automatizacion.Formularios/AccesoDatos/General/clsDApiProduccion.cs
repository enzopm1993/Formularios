using Asiservy.Automatizacion.Formularios.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.General
{
    public class clsDApiProduccion
    {
        public object ConsultarObservaciones(string Codigo)
        {
            var client = new RestClient("http://192.168.0.31:8003");
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
        public object ConsultarTallas(string Talla)
        {
            var client = new RestClient("http://192.168.0.31:8870");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);
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
            var client = new RestClient("http://192.168.0.31:8870");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);
            RestRequest request;          
            request = new RestRequest("/api/Produccion/LotesPorFecha/" + Fecha.Year+"-"+Fecha.Month+"-"+Fecha.Day+ "/", Method.GET);
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
            var client = new RestClient("http://192.168.0.31:8870");
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
            var client = new RestClient("http://192.168.0.31:8870");
            RestRequest request;
            request = new RestRequest("/api/Produccion/Barcos", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            if(response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            var Lista = JsonConvert.DeserializeObject(content);
            return Lista;
        }

        public List<InsumosProduccion> ConsultaAditivos()
        {
            var client = new RestClient("http://192.168.0.31:8870");
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
    }
}