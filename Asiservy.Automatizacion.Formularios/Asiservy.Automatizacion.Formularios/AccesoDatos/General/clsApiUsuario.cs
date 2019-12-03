using Asiservy.Automatizacion.Formularios.Models;
using Asiservy.Automatizacion.Formularios.Models.Seguridad;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                return null;
            var content = response.Content;
            var ListaUsuarios = JsonConvert.DeserializeObject(content);
            return ListaUsuarios;

        }

        public List<Usuario> ConsultaListaUsuariosSap()
        {
            var client = new RestClient("http://192.168.0.31:8870");
            var request = new RestRequest("/api/Usuarios", Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                return null;
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
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                return null;
            dynamic content = response.Content;
            if (string.IsNullOrEmpty(content))
                throw new Exception("no se pudo establecer conexión con el servicio");
            Usuario ListaUsuarios = JsonConvert.DeserializeObject<Usuario>(content);
            //var Nombre = content.Objeto.Nombre;

            return ListaUsuarios.Cedula;

        }


        public DateTime? ConsultarFechaBiometrico(string Identificacion)
        {

            

            DateTime? pdfecha = null;
            var client = new RestClient("http://192.168.0.31:8870");
            var request = new RestRequest("/api/Marcaciones/"+ Identificacion, Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                return null;
            var content = response.Content;
            dynamic Result = JsonConvert.DeserializeObject(content);
            if (Result != null && Result.Count>0)
            {
                var Resultado = Result[0];
                if (!string.IsNullOrEmpty((string)Resultado.UltimaMarcacion.Value))
                pdfecha = (DateTime)Resultado.UltimaMarcacion;
            }
            return pdfecha;

        }
        public List<Marcacion> ConsultarUltimaMarcacionxFecha(DateTime pdFecha)
        {
            var client = new RestClient("http://192.168.0.31:8870");
            var request = new RestRequest("/api/Marcaciones/" + pdFecha.ToString("yyyy-MM-dd"), Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                return null;
            var content = response.Content;
            var ListMacracaciones = JsonConvert.DeserializeObject<List<Marcacion>>(content);
            return (List<Marcacion>)ListMacracaciones;
        }

        public RespuestaGeneral CambiarClaveLogin(string dsUsuario,string dsClaveActual, string dsClave)
        {
            var client = new RestClient("http://192.168.0.31:8870");
            var request = new RestRequest("/api/Empleado/ActualizarPerfil", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new
            {
                cambiarNombre=0,
                NombreMuestra="",
                Usuario= dsUsuario,
                ClaveActual= dsClaveActual,
                CambioClave=1,
                NuevaClave= dsClave
            });

            //request.AddParameter("cambiarNombre", 0);
            //request.AddParameter("NombreMuestra", "");
            //request.AddParameter("Usuario", Usuario);
            //request.AddParameter("ClaveActual", ClaveActual);
            //request.AddParameter("CambioClave", 1);
            //request.AddParameter("NuevaClave", Clave);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                return null;
            var content = response.Content;
            var respuesta = JsonConvert.DeserializeObject<RespuestaGeneral>(content);
            return respuesta;
        }

    }
}