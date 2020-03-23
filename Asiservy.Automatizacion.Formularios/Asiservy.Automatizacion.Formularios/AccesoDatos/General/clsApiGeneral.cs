using Asiservy.Automatizacion.Formularios.AccesoDatos.App;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.General
{
    public class clsApiGeneral
    {

        public List<Comunicados> ConsultaComunicados()
        {
            var client = new RestClient("http://192.168.0.31:8870");
            var request = new RestRequest("/api/Comunicados/App", Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                return new List<Comunicados>();
            var content = response.Content;
            var ListaUsuarios = JsonConvert.DeserializeObject<List<Comunicados>>(content);
            return (List<Comunicados>)ListaUsuarios;
        }
    }
}