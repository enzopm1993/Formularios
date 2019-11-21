using Asiservy.Automatizacion.Datos.Datos;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.GenerarDatosServicio
{
    public class clsDGenerarDatosServicio
    {
        public void GenerarBarcos(BARCO model)
        {
            var client = new RestClient("http://192.168.0.31:8870");
            RestRequest request;
            request = new RestRequest("/api/Produccion/Barcos", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            dynamic Lista = JsonConvert.DeserializeObject(content);

            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {              
                foreach (var x  in Lista)
                {
                    BARCO barco = new BARCO
                    {
                        IdBarco = x.Codigo,
                        Nombre = x.Nombre,
                        CodigoSyp = x.codigo_syp,
                        EstadoRegistro=clsAtributos.EstadoRegistroActivo,
                        UsuarioIngresoLog = model.UsuarioIngresoLog,
                        TerminalIngresoLog = model.TerminalIngresoLog,
                        FechaIngresoLog = DateTime.Now                 
                    };
                    var poBarco = entities.BARCO.FirstOrDefault(y =>y.IdBarco == barco.IdBarco);

                    if(poBarco!= null)
                    {
                        poBarco.Nombre = barco.Nombre;
                        poBarco.CodigoSyp = barco.CodigoSyp;
                        poBarco.UsuarioModificacionLog = model.UsuarioIngresoLog;
                        poBarco.FechaModificacionLog = DateTime.Now;
                        poBarco.TerminalModificacionLog = model.TerminalIngresoLog;
                    }
                    else
                    {
                        entities.BARCO.Add(barco);
                    }
                    entities.SaveChanges();
                }              
            
            }

            //return Lista;
        }
    }
}