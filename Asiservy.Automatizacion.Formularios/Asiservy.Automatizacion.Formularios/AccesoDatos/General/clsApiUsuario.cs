using Asiservy.Automatizacion.Formularios.Models.Seguridad;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.General
{
    public class clsApiUsuario
    {
        public object ConsultaUsuariosSap()
        {
            var client = new RestClient("http://192.168.0.31:8870");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("/api/Usuarios", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var ListaUsuarios = JsonConvert.DeserializeObject(content);
            return ListaUsuarios;

        }

        public List<Usuario> ConsultaListaUsuariosSap()
        {
            var client = new RestClient("http://192.168.0.31:8870");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("/api/Usuarios", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var ListaUsuarios = JsonConvert.DeserializeObject<List<Usuario>>(content);
            //foreach(var x in ListaUsuarios)
            //{

            //}



            return (List<Usuario>)ListaUsuarios;

        }


    }
}