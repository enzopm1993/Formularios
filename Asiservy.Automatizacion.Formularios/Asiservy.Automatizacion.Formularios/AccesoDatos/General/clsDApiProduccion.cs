﻿using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.General
{
    public class clsDApiProduccion
    {
        public object ConsultarTallas(string Talla)
        {
            var client = new RestClient("http://192.168.0.31:8870");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);
            RestRequest request;
            if (string.IsNullOrEmpty(Talla))
             request = new RestRequest("/api/Produccion/Tallas", Method.GET);
            else
             request = new RestRequest("/api/Produccion/Tallas/"+Talla+"/", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var ListaTallas = JsonConvert.DeserializeObject(content);
            return ListaTallas;

        }
        public object ConsultarEspecies()
        {
            var client = new RestClient("http://192.168.0.31:8870");
            RestRequest request;
            request = new RestRequest("/api/Produccion/Especies", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var ListaEspecies = JsonConvert.DeserializeObject(content);
            return ListaEspecies;
        }
    }
}