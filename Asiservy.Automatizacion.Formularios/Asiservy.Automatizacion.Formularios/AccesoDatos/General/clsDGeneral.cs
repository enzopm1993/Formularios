﻿using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;




namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsDGeneral
    {
        public class Biometrico
        {
            public bool Marcacion { get; set; }
            public TimeSpan? Hora { get; set; }
        }
        public List<spConsultaCargos> ConsultaCargos(string dsCodigo)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaCargos(dsCodigo).ToList();
            }
        }

        public List<sp_GrupoEnfermedades> ConsultaCodigosGrupoSubEnfermedad(string tipo, string Grupo, string SubGrupo)
        {
            
                //entities = new ASIS_PRODEntities();
                //return entities.spConsultaCodigosEnfermedad("0").ToList();
            using(ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                
                return db.sp_GrupoEnfermedades(tipo, Grupo, SubGrupo).ToList();
            }
            
        }
        public List<spConsultaArea> ConsultaAreas(string dsCodigo)
        {

            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaArea(dsCodigo).ToList();

            }
            
        }
        public List<spConsultaRecurso> ConsultaRecursos(string dsCodigo)
        {

            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaRecurso(dsCodigo).ToList();

            }

        }
        public List<spConsultaCargosXRecursoLinea> ConsultaCargosxRecursoyLinea(string dsCodigoRecurso, string dsLinea)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaCargosXRecursoLinea(dsCodigoRecurso, dsLinea).ToList();
            }
        }
        public List<spConsultaLineaXRecursoyCentroCosto> ConsultaLineasxCCyRecurso(string dsCodigoCC,string dsCodigoRecurso)
        {

            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaLineaXRecursoyCentroCosto(dsCodigoRecurso, dsCodigoCC).ToList();

            }

        }
        public List<spConsultaLinea> ConsultaLineas(string dsCodigo)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaLinea(dsCodigo).ToList();
            }
        }
        public List<spConsultaCentroCostos> ConsultaCentroCostos()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaCentroCostos().ToList();
            }
        }
        public String ConsultarLineaUsuario(string Identificacion)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                string Linea = string.Empty;
                var Empleado =  entities.spConsutaEmpleados(Identificacion).FirstOrDefault();
                if (Empleado != null)
                {
                    var poLinea= entities.spConsultaLinea(Empleado.CODIGOLINEA).FirstOrDefault();
                    if (poLinea != null)
                    {
                        Linea = poLinea.Descripcion ?? "";
                    }
                }

                return Linea;
            }
        }

        public List<CLASIFICADOR> ConsultarEstadosSolicitudSelect()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var Estados = entities.ESTADO_SOLICITUD.Where(x=> x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).ToList();
                Estados.Add(new ESTADO_SOLICITUD { Estado = "000", Descripcion = "Todos" });
                var Lista = Estados.OrderBy(x => x.Estado).Select(x => new CLASIFICADOR {Codigo= x.Estado, Descripcion=x.Descripcion });
                
                return Lista.ToList();
            }
        }
        public Boolean ConsultarSiMarcoBiometrico(string cedula)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                DateTime FechaActual = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                var resultado = db.spConsultaUltimaMarcacionBiometrico(cedula).FirstOrDefault();
                if (resultado == null){
                    return false;
                }
                if (resultado.Marcacion.Value >= FechaActual)
                    return true;
                else
                    return false;
            }
        }
        public Biometrico ConsultarBiometricoxFecha(string cedula,DateTime Fecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
               
                spConsultaMarcacionBiometricoxFecha resultado = db.spConsultaMarcacionBiometricoxFecha(cedula,Fecha).FirstOrDefault();

                if (resultado == null)
                {
                    return new Biometrico { Marcacion=false,Hora=null };
                }
                else
                {
                    var respuesta= resultado.IngresoMarca == null? new Biometrico { Marcacion = false, Hora = null }:new Biometrico { Marcacion = true, Hora = resultado.IngresoMarca.Value.TimeOfDay };
                    return respuesta;
                    
                }
            }
        }

        public string EnvioCorreo(string Cedula, string asunto, string mensaje,bool RRHH)
        {
            try
            {
                using (ASIS_PRODEntities db = new ASIS_PRODEntities())
                {
                    var Mensaje = db.spEnvioCorreo(Cedula,asunto,mensaje,RRHH).FirstOrDefault();
                    return Mensaje;
                }

                //string Correo = System.Configuration.ConfigurationSettings.AppSettings["Correo"];
                //string Clave = System.Configuration.ConfigurationSettings.AppSettings["CorreoClave"];

                //string CorreosDestino= string.Empty;
                //if(!string.IsNullOrEmpty(Empleado.CorreoEmpresa) && Empleado.CorreoEmpresa != "*")
                //{
                //    CorreosDestino = Empleado.CorreoEmpresa;
                //}
                //if(!string.IsNullOrEmpty(Empleado.CorreoPersonal) && Empleado.CorreoPersonal != "*")
                //{
                //    if(!string.IsNullOrEmpty(CorreosDestino) && Empleado.CorreoEmpresa != Empleado.CorreoPersonal)
                //    {
                //        CorreosDestino = CorreosDestino + "," + Empleado.CorreoPersonal;
                //    }
                //    else
                //    {
                //        CorreosDestino = Empleado.CorreoPersonal;
                //    }
                //}
                //if (!string.IsNullOrEmpty(CorreosDestino))
                //{
                //    clsDParametro clsDParametro = new clsDParametro();
                //    string text = "MENSAJE DEL SISTEMA\n\n" + mensaje;

                //    AlternateView plainView =
                //        AlternateView.CreateAlternateViewFromString(text,
                //                                Encoding.UTF8,
                //                                MediaTypeNames.Text.Plain);


                //    string html = "<H3>MENSAJE DEL SISTEMA</H3>" + mensaje +
                //  "<img src='cid:imagen' />";
                //    AlternateView htmlView =
                //    AlternateView.CreateAlternateViewFromString(html,
                //                Encoding.UTF8,
                //                MediaTypeNames.Text.Html);

                //    //LinkedResource img = new LinkedResource(@"C:\Desarrollo\Asiservy.Automatizacion.Formularios\Asiservy.Automatizacion.Formularios\Content\images\asilogo.jpg",
                //    //        MediaTypeNames.Image.Jpeg);
                //    //img.ContentId = "imagen";
                //    //htmlView.LinkedResources.Add(img);

                //    MailMessage correo = new MailMessage(Correo, CorreosDestino);
                //    if(RRHH)
                //    {
                //        clsDClasificador clsDClasificador = new clsDClasificador();
                //        var correos = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoCorreosElectronicosCopias);
                //        string correoCopias = string.Empty;
                //        foreach (var c in correos)
                //        {
                //            correoCopias = correoCopias + c.Descripcion + ",";
                //        }
                //        if(string.IsNullOrEmpty(correoCopias))
                //            correo.Subject = correoCopias;

                //    }
                //    correo.Subject = asunto;
                //    correo.AlternateViews.Add(htmlView);
                //    correo.AlternateViews.Add(plainView);

                //    SmtpClient servidor = new SmtpClient("smtp.office365.com", 587);
                //    NetworkCredential credenciales = new NetworkCredential(Correo, Clave);
                //    servidor.Credentials = credenciales;
                //    servidor.EnableSsl = true;
                //    servidor.Send(correo);
                //    return "Correo Enviado con Éxito";
                //}
                //else
                //{
                //    return "Empleado no tiene correo electronico asignado";
                //}

            }           
            catch (SmtpException)
            {
                return "Error en envió de correo electrónico";

            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }


        public List<MANTENIMIENTO_PREPARACION> ConsultarMantenimientoPreparacion()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {               
                return entities.MANTENIMIENTO_PREPARACION.ToList();
            }
        }

        public string getDataBase()
        {
            String dbname = "";
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASIS_PRODEntities"].ConnectionString;
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(?<=\bconnection string="")[^""]*");
                System.Text.RegularExpressions.Match match = regex.Match(connectionString);
                string title = match.Value;

                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(title);

                dbname = builder.InitialCatalog;
            }
            catch (Exception)
            {
                dbname = "";
            }

            return dbname;
        }


        public List<spConsultaNotificacionesSiaa> ConsultaNotificaciones (string Cedula)
        {
            using ( ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaNotificacionesSiaa(Cedula).ToList();
            }
        }

    }
}