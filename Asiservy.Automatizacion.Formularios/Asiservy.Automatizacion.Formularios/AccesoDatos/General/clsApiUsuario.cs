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
            var request = new RestRequest("/api/Usuarios", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var ListaUsuarios = JsonConvert.DeserializeObject<List<Usuario>>(content);
            return (List<Usuario>)ListaUsuarios;
        }

        public string ConsultaUsuarioEspecificoSap(string usuario, string clave)
        {
            var client = new RestClient("http://192.168.0.31:8870");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("/api/Login", Method.POST);
            request.AddParameter("usuario", usuario);
            request.AddParameter("clave", clave);
            IRestResponse response = client.Execute(request);
            dynamic content = response.Content;
            var ListaUsuarios = JsonConvert.DeserializeObject<Usuario>(content);
            var Nombre = content.Objeto.Nombre;
            return "";

        }
    }
}