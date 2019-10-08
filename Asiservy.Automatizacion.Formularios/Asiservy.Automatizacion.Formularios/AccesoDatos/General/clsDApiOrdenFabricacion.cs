using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.General
{
    public class clsDApiOrdenFabricacion
    {

        public object ConsultaLotesPorOrdenFabricacionLinea(int OrdernFabricacion, string Linea)
        {
            var client = new RestClient("http://192.168.0.31:8870");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("/api/Produccion/LotesPorOrdenLinea/" + OrdernFabricacion+"/"+Linea, Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var ListaUsuarios = JsonConvert.DeserializeObject(content);
            return ListaUsuarios;           
        }
        public object ConsultaOrdenFabricacionPorFechaProduccion(DateTime FechaProduccion)
        {
            var client = new RestClient("http://192.168.0.31:8870");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("/api/Produccion/OrdenesFabricacionPorFecha/"+FechaProduccion.ToString("yyyy-MM-dd"), Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var ListaOrdenes = JsonConvert.DeserializeObject(content);
            return ListaOrdenes;

        }
       
    }
}