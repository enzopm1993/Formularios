using Asiservy.Automatizacion.Formularios.Models;
using Asiservy.Automatizacion.Formularios.Models.Seguridad;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.General
{
    public class clsApiUsuario
    {
        public object ConsultaUsuariosSap()
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);
         
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
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            var request = new RestRequest("/api/Usuarios", Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                return null;
            var content = response.Content;
            var ListaUsuarios = JsonConvert.DeserializeObject<List<Usuario>>(content);
            return ListaUsuarios;
        }

        public string ConsultaUsuarioEspecificoSap(string usuario, string clave)
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);
         
            var request = new RestRequest("/api/Login", Method.POST);
            request.AddParameter("usuario", usuario);
            request.AddParameter("clave", clave);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                return null;
            dynamic content = response.Content;
            if (string.IsNullOrEmpty(content))
                return ("no se pudo establecer conexión con el servicio");
            Usuario ListaUsuarios = JsonConvert.DeserializeObject<Usuario>(content);
           
            return ListaUsuarios.Cedula;

        }


        public DateTime? ConsultarFechaBiometrico(string Identificacion)
        {

            

            DateTime? pdfecha = null;
            var client = new RestClient(clsAtributos.BASE_URL_WS);
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
                {
                    pdfecha = (DateTime)Resultado.UltimaMarcacion;
                }
            }
            return pdfecha;

        }
        public List<Marcacion> ConsultarUltimaMarcacionxFecha(DateTime pdFecha)
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);
            var request = new RestRequest("/api/Marcaciones/" + pdFecha.ToString("yyyy-MM-dd"), Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                return null;
            var content = response.Content;
            var ListMacracaciones = JsonConvert.DeserializeObject<List<Marcacion>>(content);
            return ListMacracaciones;
        }

        public RespuestaGeneral CambiarClaveLogin(string dsUsuario,string dsClaveActual, string dsClave)
        {
            var client = new RestClient(clsAtributos.BASE_URL_WS);
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
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                return null;
            var content = response.Content;
            var respuesta = JsonConvert.DeserializeObject<RespuestaGeneral>(content);
            return respuesta;
        }

    }
}